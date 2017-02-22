using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Input;
using Codeplex.Reactive;
using EpicTTS.Utility;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.ViewModels
{
    public class MainViewModel : IDisposable
    {
        private readonly SpeechSynthesizer _synthesizer;
        public IEnumerable<InstalledVoice> Voices { get; }
        public ICommand SpeakCommand { get; private set; }
        public ICommand StopSpeakingCommand { get; private set; }
        public ICommand PauseSpeakingCommand { get; private set; }
        public ObservableCollection<IExport> Exports { get; }

        public ReactiveProperty<IExport> SelectedExport { get; } = new ReactiveProperty<IExport>();

        public ReactiveProperty<SynthesizerState> State { get; } = new ReactiveProperty<SynthesizerState>();

        public MainViewModel(Options options)
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.StateChanged += (_, __) => State.Value = _synthesizer.State;

            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
            SelectedVoice.Value = Voices.First();

            Document = new TextDocumentViewModel();
            if (File.Exists(options.InputPath))
                Document.Open(options.InputPath);

            SelectedExport.Subscribe(OnExportChanged);

            var exportToFilePath = "";
            if (!String.IsNullOrWhiteSpace(options.OutputPath))
                exportToFilePath = options.OutputPath;
            else if (!String.IsNullOrWhiteSpace(options.InputPath))
                exportToFilePath = options.InputPath + ".wav";
            Exports = new ObservableCollection<IExport>
            {
                new ExportToDefaultAudioDeviceViewModel(),
                new ExportToFileViewModel{FilePath = { Value = exportToFilePath}},
            };
            foreach (var export in Exports)
            {
                export.SpeechSynthesizer.Value = _synthesizer;
            }

            if (!String.IsNullOrWhiteSpace(options.OutputPath))
                SelectedExport.Value = Exports[1];
            else
                SelectedExport.Value = Exports[0];

            SpeakCommand = new RelayCommand(Speak);
            StopSpeakingCommand = new RelayCommand(StopSpeaking);
            PauseSpeakingCommand = new RelayCommand(PauseSpeaking);

            SelectedVoice.Subscribe(voice => _synthesizer.SelectVoice(voice.VoiceInfo.Name));
        }

        private void OnExportChanged(IExport value)
        {
            if (State.Value != SynthesizerState.Ready)
            {
                StopSpeaking(null);
                foreach (var export in Exports)
                {
                    export.IsSelected.Value = false;
                }
            }

            if (value != null)
            {
                value.IsSelected.Value = true;
                value.ExportCommand.Execute();
            }
        }

        private void PauseSpeaking(object obj)
        {
            _synthesizer.Pause();
        }
        
        public void Speak()
        {
            _synthesizer.Speak(Document.AsPrompt());
        }

        private void Speak(object obj)
        {
            if (_synthesizer.State == SynthesizerState.Paused)
                _synthesizer.Resume();
            else
            {
                SelectedExport.Value.ExportCommand.Execute();
                _synthesizer.SpeakAsync(Document.AsPrompt());
            }
        }

        private void StopSpeaking(object obj)
        {
            if (_synthesizer.State == SynthesizerState.Paused)
                _synthesizer.Resume();
            _synthesizer.SpeakAsyncCancelAll();
        }

        public ReactiveProperty<InstalledVoice> SelectedVoice { get; } = new ReactiveProperty<InstalledVoice>();

        public TextDocumentViewModel Document { get; }

        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
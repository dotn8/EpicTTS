using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Input;
using EpicTTS.Utility;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.Models
{
    public class MainModel : ObservableObject, IDisposable
    {
        private SpeechSynthesizer _synthesizer;
        private InstalledVoice _selectedVoice;
        private TextDocument _document;
        private IExport _selectedExport;
        public IList<InstalledVoice> Voices { get; private set; }
        public ICommand SpeakCommand { get; private set; }
        public ICommand StopSpeakingCommand { get; private set; }
        public ICommand PauseSpeakingCommand { get; private set; }
        public ObservableCollection<IExport> Exports { get; private set; }

        public IExport SelectedExport
        {
            get { return GetProperty(ref _selectedExport); }
            set
            {
                if (SetProperty(ref _selectedExport, value))
                {
                    if (State != SynthesizerState.Ready)
                    {
                        StopSpeaking(null);
                        Exports.ForEach(export => export.IsSelected = false);
                        value.IsSelected = true;
                    }
                    value.Export();
                }
            }
        }

        public SynthesizerState State
        {
            get { return _synthesizer.State; }
        }

        public MainModel(Options options)
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.StateChanged += StateChanged;

            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
            SelectedVoice = Voices[0];

            var commandLineArguments = Environment.GetCommandLineArgs();

            Document = new TextDocument();
            if (File.Exists(options.InputPath))
                Document.Open(options.InputPath);

            var exportToFilePath = "";
            if (!String.IsNullOrWhiteSpace(options.OutputPath))
                exportToFilePath = options.OutputPath;
            else if (commandLineArguments.Length > 1)
                exportToFilePath = commandLineArguments[1] + ".wav";
            Exports = new ObservableCollection<IExport>
            {
                new ExportToDefaultAudioDevice(),
                new ExportToFile{FilePath = exportToFilePath},
            };
            Exports.ForEach(export => export.SpeechSynthesizer = _synthesizer);
            if (!String.IsNullOrWhiteSpace(options.OutputPath))
                SelectedExport = Exports[1];
            else
                SelectedExport = Exports[0];

            SpeakCommand = new RelayCommand(Speak);
            StopSpeakingCommand = new RelayCommand(StopSpeaking);
            PauseSpeakingCommand = new RelayCommand(PauseSpeaking);
        }

        private void PauseSpeaking(object obj)
        {
            _synthesizer.Pause();
        }

        private void StateChanged(object sender, StateChangedEventArgs e)
        {
            OnPropertyChanged("State");
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
                SelectedExport.Export();
                _synthesizer.SpeakAsync(Document.AsPrompt());
            }
        }

        private void StopSpeaking(object obj)
        {
            if (_synthesizer.State == SynthesizerState.Paused)
                _synthesizer.Resume();
            _synthesizer.SpeakAsyncCancelAll();
        }

        public InstalledVoice SelectedVoice
        {
            get { return GetProperty(ref _selectedVoice); }
            set
            {
                SetProperty(ref _selectedVoice, value);
                _synthesizer.SelectVoice(_selectedVoice.VoiceInfo.Name);
            }
        }

        public TextDocument Document
        {
            get { return GetProperty(ref _document); }
            set { SetProperty(ref _document, value); }
        }

        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
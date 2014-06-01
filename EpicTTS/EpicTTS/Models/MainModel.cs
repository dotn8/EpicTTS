using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        StopSpeaking(null);
                    value.Export();
                }
            }
        }

        public SynthesizerState State
        {
            get { return _synthesizer.State; }
        }

        public MainModel()
        {
            Initialize();
        }

        private void PauseSpeaking(object obj)
        {
            _synthesizer.Pause();
        }

        private void StateChanged(object sender, StateChangedEventArgs e)
        {
            OnPropertyChanged("State");
        }

        private void Speak(object obj)
        {
            if (_synthesizer.State == SynthesizerState.Paused)
                _synthesizer.Resume();
            else
                _synthesizer.SpeakAsync(Document.AsPrompt());
        }

        private void StopSpeaking(object obj)
        {
            if (_synthesizer.State == SynthesizerState.Paused)
                _synthesizer.Resume();
            _synthesizer.SpeakAsyncCancelAll();
        }

        private void Initialize()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.StateChanged += StateChanged;

            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
            SelectedVoice = Voices[0];

            var commandLineArguments = Environment.GetCommandLineArgs();

            Document = new TextDocument();
            if (commandLineArguments.Length > 1)
                Document.Open(commandLineArguments[1]);

            var exportToFilePath = "";
            if (commandLineArguments.Length > 2)
                exportToFilePath = commandLineArguments[2];
            else if (commandLineArguments.Length > 1)
                exportToFilePath = commandLineArguments[1] + ".wav";
            Exports = new ObservableCollection<IExport>
            {
                new ExportToDefaultAudioDevice(),
                new ExportToFile{FilePath = exportToFilePath},
            };
            Exports.ForEach(export => export.SpeechSynthesizer = _synthesizer);
            SelectedExport = Exports[0];

            SpeakCommand = new RelayCommand(Speak);
            StopSpeakingCommand = new RelayCommand(StopSpeaking);
            PauseSpeakingCommand = new RelayCommand(PauseSpeaking);
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
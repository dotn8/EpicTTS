using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.Models
{
    public class MainModel : ObservableObject, IDisposable
    {
        private SpeechSynthesizer _synthesizer;
        private InstalledVoice _selectedVoice;
        private TextDocument _document;
        public IList<InstalledVoice> Voices { get; private set; }
        public ICommand SpeakCommand { get; private set; }
        public ICommand StopSpeakingCommand { get; private set; }
        public ICommand PauseSpeakingCommand { get; private set; }

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
            InitializeSynthesizer();
            InitializeVoices();
            InitializeSelectedVoice();
            InitializeDocument();
            InitializeCommands();
        }

        public InstalledVoice SelectedVoice
        {
            get { return GetProperty(ref _selectedVoice); }
            set
            {
                SetProperty(out _selectedVoice, value);
                _synthesizer.SelectVoice(_selectedVoice.VoiceInfo.Name);
            }
        }

        private void InitializeSynthesizer()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.StateChanged += StateChanged;
        }

        private void InitializeVoices()
        {
            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
        }

        private void InitializeSelectedVoice()
        {
            SelectedVoice = Voices[0];
        }

        private void InitializeDocument()
        {
            Document = new TextDocument();
            var commandLineArguments = Environment.GetCommandLineArgs();
            if (commandLineArguments.Length > 1)
                Document.Open(commandLineArguments[1]);
        }

        private void InitializeCommands()
        {
            SpeakCommand = new RelayCommand(Speak);
            StopSpeakingCommand = new RelayCommand(StopSpeaking);
            PauseSpeakingCommand = new RelayCommand(PauseSpeaking);
        }

        public TextDocument Document
        {
            get { return GetProperty(ref _document); }
            set { SetProperty(out _document, value); }
        }

        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
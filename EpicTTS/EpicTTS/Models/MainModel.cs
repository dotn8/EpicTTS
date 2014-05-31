using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Windows.Input;
using EpicTTS.Annotations;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.Models
{
    public class MainModel : INotifyPropertyChanged, IDisposable
    {
        private readonly SpeechSynthesizer _synthesizer;
        private InstalledVoice _selectedVoice;
        private string _text;
        public IList<InstalledVoice> Voices { get; private set; }
        public ICommand SpeakCommand { get; private set; }
        public ICommand StopSpeakingCommand { get; private set; }
        public ICommand PauseSpeakingCommand { get; private set; }
        public SynthesizerState State { get { return _synthesizer.State; } }

        public MainModel()
        {
            _synthesizer = new SpeechSynthesizer();
            Initialize();
            SpeakCommand = new RelayCommand(Speak);
            StopSpeakingCommand = new RelayCommand(StopSpeaking);
            PauseSpeakingCommand = new RelayCommand(PauseSpeaking);
            _synthesizer.StateChanged += StateChanged;
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
                _synthesizer.SpeakAsync(Text);
        }

        private void StopSpeaking(object obj)
        {
            _synthesizer.SpeakAsyncCancelAll();
        }

        private void Initialize()
        {
            InitializeVoices();
        }

        public InstalledVoice SelectedVoice
        {
            get { return _selectedVoice; }
            set
            {
                if (Equals(value, _selectedVoice)) return;
                _selectedVoice = value;
                _synthesizer.SelectVoice(_selectedVoice.VoiceInfo.Name);
                OnPropertyChanged();
            }
        }

        private void InitializeVoices()
        {
            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using EpicTTS.Annotations;

namespace EpicTTS.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private readonly SpeechSynthesizer _synthesizer;
        private InstalledVoice _selectedVoice;
        private string _text;
        public IList<InstalledVoice> Voices { get; private set; }

        public MainModel()
        {
            _synthesizer = new SpeechSynthesizer();
            Initialize();
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
    }
}
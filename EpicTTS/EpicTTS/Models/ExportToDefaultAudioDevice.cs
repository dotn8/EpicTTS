using System.Speech.Synthesis;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.Models
{
    public class ExportToDefaultAudioDevice : ObservableObject, IExport
    {
        private SpeechSynthesizer _speechSynthesizer;
        private bool _isSelected;

        public SpeechSynthesizer SpeechSynthesizer
        {
            get { return GetProperty(ref _speechSynthesizer); }
            set { SetProperty(ref _speechSynthesizer, value); }
        }

        public bool IsSelected
        {
            get { return GetProperty(ref _isSelected); }
            set { SetProperty(ref _isSelected, value); }
        }

        public string Description
        {
            get { return "Export to default audio device"; }
        }

        public ICommand BrowseCommand { get; private set; }

        public ExportToDefaultAudioDevice()
        {
            BrowseCommand = new RelayCommand(obj => Export());
        }

        public void Export()
        {
            SpeechSynthesizer.SetOutputToDefaultAudioDevice();
        }
    }
}
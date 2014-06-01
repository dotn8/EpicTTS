using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Win32;

namespace EpicTTS.Models
{
    public class ExportToFile : ObservableObject, IExport
    {
        private string _filePath;
        private SpeechSynthesizer _speechSynthesizer;
        public ICommand BrowseCommand { get; set; }

        public string Description
        {
            get { return "Export to " + FilePath; }
        }

        public SpeechSynthesizer SpeechSynthesizer
        {
            get { return GetProperty(ref _speechSynthesizer); }
            set { SetProperty(ref _speechSynthesizer, value); }
        }

        public string FilePath
        {
            get { return GetProperty(ref _filePath); }
            set
            {
                if (SetProperty(ref _filePath, value))
                    OnPropertyChanged("Description");
            }
        }

        public ExportToFile()
        {
            _filePath = "";
            BrowseCommand = new RelayCommand(obj => Browse());
        }

        public void Export()
        {
            if (String.IsNullOrWhiteSpace(FilePath) || !new FileInfo(FilePath).Directory.Exists)
                Browse();
            SpeechSynthesizer.SetOutputToWaveFile(FilePath);
        }

        private void Browse()
        {
            var dlg = new SaveFileDialog {DefaultExt = ".wav", Filter = "Text documents (.wav)|*.wav"};
            var result = dlg.ShowDialog();
            if (result == true)
            {
                FilePath = dlg.FileName;
            }
        }
    }
}
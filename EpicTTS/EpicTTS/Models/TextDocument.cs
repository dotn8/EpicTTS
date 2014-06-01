using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows.Input;
using System.Windows.Interop;
using FirstFloor.ModernUI.Presentation;

namespace EpicTTS.Models
{
    public class TextDocument : ObservableObject
    {
        private string _filePath;
        private string _text;
        public ICommand BrowseCommand { get; private set; }

        public string Text
        {
            get { return GetProperty(ref _text); }
            set { SetProperty(out _text, value); }
        }

        public string FilePath
        {
            get { return GetProperty(ref _filePath); }
            set { SetProperty(out _filePath, value); }
        }

        public TextDocument()
        {
            Text = "";
            BrowseCommand = new RelayCommand(obj => Browse());
        }

        private void Browse()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog {DefaultExt = ".txt", Filter = "Text documents (.txt)|*.txt"};
            var result = dlg.ShowDialog();
            if (result == true)
            {
                FilePath = dlg.FileName;
                Open();
            }
        }

        public void Open(string path)
        {
            FilePath = path;
            Open();
        }

        public void Open()
        {
            Text = File.ReadAllText(FilePath);
        }

        public Prompt AsPrompt()
        {
            return new Prompt(Text);
        }
    }
}
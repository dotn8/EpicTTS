using System;
using System.IO;
using System.Security.AccessControl;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EpicTTS.Utility;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Win32;

namespace EpicTTS.Models
{
    public class ExportToFile : ObservableObject, IExport
    {
        private string _filePath;
        private SpeechSynthesizer _speechSynthesizer;
        private bool _isSelected;
        public ICommand BrowseCommand { get; set; }
        public ICommand ShowContextMenuCommand { get; set; }

        public string Description
        {
            get { return "Export to file"; }
        }

        public bool IsSelected
        {
            get { return GetProperty(ref _isSelected); }
            set { SetProperty(ref _isSelected, value); }
        }

        public SpeechSynthesizer SpeechSynthesizer
        {
            get { return GetProperty(ref _speechSynthesizer); }
            set
            {
                var oldValue = _speechSynthesizer;
                if (SetProperty(ref _speechSynthesizer, value))
                {
                    if (oldValue != null)
                        oldValue.SpeakCompleted -= OnSpeakCompleted;
                    if (value != null)
                        value.SpeakCompleted += OnSpeakCompleted;
                }
            }
        }

        private void OnSpeakCompleted(object sender, SpeakCompletedEventArgs speakCompletedEventArgs)
        {
            SpeechSynthesizer.SetOutputToNull();
        }

        public string FilePath
        {
            get { return GetProperty(ref _filePath); }
            set { SetProperty(ref _filePath, value); }
        }

        public ExportToFile()
        {
            _filePath = "";
            BrowseCommand = new RelayCommand(obj => Browse());
            ShowContextMenuCommand = new RelayCommand(ShowContextMenu);
        }

        private void ShowContextMenu(object obj)
        {
            var button = (Button) obj;
            var shellContextMenu = new ShellContextMenu();
            shellContextMenu.ShowContextMenu(new []{new FileInfo(FilePath)}, button.PointToScreen(new Point(0, 0)));
        }

        public void Export()
        {
            if (String.IsNullOrWhiteSpace(FilePath) || !new FileInfo(FilePath).Directory.Exists)
                Browse();
            if (String.IsNullOrWhiteSpace(FilePath))
                SpeechSynthesizer.SetOutputToNull();
            else
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
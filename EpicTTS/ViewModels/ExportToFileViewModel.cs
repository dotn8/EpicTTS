using System;
using System.IO;
using System.Reactive.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using Codeplex.Reactive;
using EpicTTS.Utility;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Win32;

namespace EpicTTS.ViewModels
{
    public class ExportToFileViewModel : IExport
    {
        public ReactiveProperty<string> FilePath { get; private set; }
        public ReactiveProperty<SpeechSynthesizer> SpeechSynthesizer { get; private set; }
        public ReactiveProperty<bool> IsSelected { get; private set; }
        public ReactiveCommand ExportCommand { get; private set; }
        public ReactiveCommand BrowseCommand { get; private set; }
        public ReactiveCommand ShellContextMenuCommand { get; private set; }
        public ReactiveProperty<string> Description { get; private set; }

        private void OnSpeakCompleted(SpeakCompletedEventArgs speakCompletedEventArgs)
        {
            SpeechSynthesizer.Value.SetOutputToNull();
        }

        public ExportToFileViewModel()
        {
            FilePath = new ReactiveProperty<string>("");
            SpeechSynthesizer = new ReactiveProperty<SpeechSynthesizer>();
            IsSelected = new ReactiveProperty<bool>();
            Description = new ReactiveProperty<string>("Export to file");
            ExportCommand = new ReactiveCommand();
            ExportCommand.Subscribe(Export);
            BrowseCommand = new ReactiveCommand();
            BrowseCommand.Subscribe(Browse);
            ShellContextMenuCommand = new ReactiveCommand();
            ShellContextMenuCommand.Subscribe(ShowContextMenu);

            SpeechSynthesizer.Select(speechSynthesizer =>
                Observable.FromEvent<EventHandler<SpeakCompletedEventArgs>, SpeakCompletedEventArgs>(
                    handler => speechSynthesizer.SpeakCompleted += handler,
                    handler => speechSynthesizer.SpeakCompleted -= handler))
                .Merge()
                .Subscribe(OnSpeakCompleted);

        }

        private void ShowContextMenu(object obj)
        {
            var button = (Button) obj;
            var shellContextMenu = new ShellContextMenu();
            shellContextMenu.ShowContextMenu(new []{new FileInfo(FilePath.Value)}, button.PointToScreen(new Point(0, 0)));
        }

        public void Export(object obj)
        {
            if (String.IsNullOrWhiteSpace(FilePath.Value) || !new FileInfo(FilePath.Value).Directory.Exists)
                Browse(obj);
            if (String.IsNullOrWhiteSpace(FilePath.Value))
                SpeechSynthesizer.Value.SetOutputToNull();
            else
                SpeechSynthesizer.Value.SetOutputToWaveFile(FilePath.Value);
        }

        private void Browse(object obj)
        {
            var dlg = new SaveFileDialog {DefaultExt = ".wav", Filter = "Text documents (.wav)|*.wav"};
            var result = dlg.ShowDialog();
            if (result == true)
            {
                FilePath.Value = dlg.FileName;
            }
        }
    }
}
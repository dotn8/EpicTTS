using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using Codeplex.Reactive;
using EpicTTS.Utility;

namespace EpicTTS.ViewModels
{
    public class TextDocumentViewModel
    {
        public ReactiveCommand BrowseCommand { get; private set; }
        public ReactiveCommand ShowContextMenuCommand { get; set; }

        public ReactiveProperty<string> Text { get; private set; }
        public ReactiveProperty<string> FilePath { get; private set; }

        public TextDocumentViewModel()
        {
            Text = new ReactiveProperty<string>("");
            FilePath = new ReactiveProperty<string>("");
            BrowseCommand = new ReactiveCommand();
            BrowseCommand.Subscribe(Browse);
            ShowContextMenuCommand = new ReactiveCommand();
            ShowContextMenuCommand.Subscribe(ShowContextMenu);
        }

        private void ShowContextMenu(object obj)
        {
            if (!File.Exists(FilePath.Value))
                return;
            var button = (Button) obj;
            var shellContextMenu = new ShellContextMenu();
            shellContextMenu.ShowContextMenu(new[] { new FileInfo(FilePath.Value) }, button.PointToScreen(new Point(0, 0)));
        }

        private void Browse(object obj)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog {DefaultExt = ".txt", Filter = "Text documents (.txt)|*.txt"};
            var result = dlg.ShowDialog();
            if (result == true)
            {
                FilePath.Value = dlg.FileName;
                Open();
            }
        }

        public void Open(string path)
        {
            FilePath.Value = path;
            Open();
        }

        public void Open()
        {
            Text.Value = File.ReadAllText(FilePath.Value);
        }

        public Prompt AsPrompt()
        {
            return new Prompt(Text.Value);
        }
    }
}
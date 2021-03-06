using System;
using System.Speech.Synthesis;
using Codeplex.Reactive;

namespace EpicTTS.ViewModels
{
    public class ExportToDefaultAudioDeviceViewModel : IExport
    {
        public ReactiveProperty<SpeechSynthesizer> SpeechSynthesizer { get; private set; }
        public ReactiveProperty<bool> IsSelected { get; private set; }
        public ReactiveProperty<string> Description { get; private set; }
        public ReactiveCommand ExportCommand { get; private set; }

        public ExportToDefaultAudioDeviceViewModel()
        {
            SpeechSynthesizer = new ReactiveProperty<SpeechSynthesizer>();
            IsSelected = new ReactiveProperty<bool>();
            Description = new ReactiveProperty<string>("Export to default audio device");

            ExportCommand = new ReactiveCommand();
            ExportCommand.Subscribe(Export);
        }

        public void Export(object obj)
        {
            SpeechSynthesizer.Value.SetOutputToDefaultAudioDevice();
        }
    }
}
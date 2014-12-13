using System.Speech.Synthesis;
using Codeplex.Reactive;

namespace EpicTTS.Models
{
    public interface IExport
    {
        ReactiveProperty<SpeechSynthesizer> SpeechSynthesizer { get; }
        ReactiveProperty<bool> IsSelected { get; }
        ReactiveProperty<string> Description { get; }
        ReactiveCommand ExportCommand { get; }
    }
}
using System.ComponentModel;
using System.Speech.Synthesis;

namespace EpicTTS.Models
{
    public interface IExport : INotifyPropertyChanged
    {
        SpeechSynthesizer SpeechSynthesizer { get; set; }
        void Export();
        string Description { get; }
    }
}
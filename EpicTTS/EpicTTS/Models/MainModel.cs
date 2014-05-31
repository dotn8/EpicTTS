using System.Collections.Generic;
using System.Speech.Synthesis;

namespace EpicTTS.Models
{
    public class MainModel
    {
        private readonly SpeechSynthesizer _synthesizer;
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

        private void InitializeVoices()
        {
            Voices = new List<InstalledVoice>(_synthesizer.GetInstalledVoices());
        }
    }
}
using System.IO;
using System.Speech.Synthesis;

namespace EpicTTS.Models
{
    public class TextDocument
    {
        public string Text { get; set; }
        public string FilePath { get; set; }

        public TextDocument()
        {
            Text = "";
        }

        public void Open(string path)
        {
            FilePath = path;
            Reload();
        }

        public void Reload()
        {
            Text = File.ReadAllText(FilePath);
        }

        public Prompt AsPrompt()
        {
            return new Prompt(Text);
        }
    }
}
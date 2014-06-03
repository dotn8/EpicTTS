using CommandLine;
using CommandLine.Text;

namespace EpicTTS
{
    public class Options
    {
        [Option('h', "headless", DefaultValue = false, HelpText = "Specifies whether or not to run the program headlessly.")]
        public bool Headless { get; set; }

        [Option('i', "input", HelpText = "Specifies the path to the input file to be spoken.")]
        public string InputPath { get; set; }

        [Option('o', "output", HelpText = "Specifies the path to the output file to be spoken.")]
        public string OutputPath { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
using CommandLine;

namespace consoleApp
{
    public class InputOptions
    {
        [Option('p', "pathToWordsFile")]
        public string PathToWordsFile { get; set; }

        [Option('o', "outputPath")]
        public string OutputPath { get; set; }

        [Option('n', "numberOfCardsToProcess")]
        public int? NumberOfCardsToProcess { get; set; }
    }
}

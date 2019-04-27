using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace consoleApp
{
    public class InputOptions
    {
        [Option('p', "pathToWordsFile")]
        public string PathToWordsFile { get; set; }

        [Option('o', "outputPath")]
        public string OutputPath { get; set; }
    }
}

using CommandLine;

namespace rgbamerge
{
    public class Options
    {
        [Option('r', "red", Required = false, HelpText = "Red channel image")]
        public string Red { get; set; }
        [Option('g', "green", Required = false, HelpText = "Green channel image")]
        public string Green { get; set; }
        [Option('b', "blue", Required = false, HelpText = "Blue channel image")]
        public string Blue { get; set; }
        [Option('a', "alpha", Required = false, HelpText = "Alpha channel image")]
        public string Alpha { get; set; }
        [Option('o', "output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }
    }
}
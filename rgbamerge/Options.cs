using CommandLine;

namespace rgbamerge
{
    public class Options
    {
        [Option('r', "red", Required = false, HelpText = "Red channel image or color as byte number 0-255")]
        public string Red { get; set; }
        [Option('g', "green", Required = false, HelpText = "Green channel image or color as byte number 0-255")]
        public string Green { get; set; }
        [Option('b', "blue", Required = false, HelpText = "Blue channel image or color as byte number 0-255")]
        public string Blue { get; set; }
        [Option('a', "alpha", Required = false, HelpText = "Alpha channel image or color as byte number 0-255")]
        public string Alpha { get; set; }
        [Option('o', "output", Required = true, HelpText = "Output image path")]
        public string Output { get; set; }
    }
}
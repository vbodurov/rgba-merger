using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;

namespace rgbamerge
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Execute);
        }
        static void Execute(Options op)
        {
            IOutput output = new Output();
            if (string.IsNullOrWhiteSpace(op.Output))
            {
                output.Error("Output file name is required");
                return;
            }
            try
            {
                Path.GetFullPath(op.Output);
            }
            catch (Exception)
            {
                output.Error("Output file name is invalid");
                return;
            }
            var io = CreateInnerOptions(output, op);
            IMerger merger = new Merger(output, io);
            merger.Merge();
        }
        static InnerOptions CreateInnerOptions(IOutput output, Options op)
        {
            var io = new InnerOptions();
            CreateInnerOption(output, () => op.Red, s => io.File_R = s, n => io.Color_R = n, "Red");
            CreateInnerOption(output, () => op.Green, s => io.File_G = s, n => io.Color_G = n, "Green");
            CreateInnerOption(output, () => op.Blue, s => io.File_B = s, n => io.Color_B = n, "Blue");
            CreateInnerOption(output, () => op.Alpha, s => io.File_A = s, n => io.Color_A = n, "Alpha");
            io.File_O = op.Output.ToLowerInvariant();
            if (!io.File_O.EndsWith(".png")) io.File_O += ".png";
            var dir = Path.GetDirectoryName(op.Output);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return io;
        }
        static void CreateInnerOption(IOutput output, Func<string> get, Action<string> setFile, Action<byte> setColor, string label)
        {
            var src = (get() ?? "").Trim();
            if (int.TryParse(src, out var n))
            {
                setFile(null);
                if(n < 0 || n > 0xFF) output.Warning($"WARNING: The value {n} for channel {label} is outside of 0-255 range");
               var value = (byte) Math.Min(Math.Max(n, 0), 255);
                output.Info($"Channel {label} set to value {value}");
                setColor(value);
            }
            else if(src == "")
            {
                output.Info($"Channel {label} set to value 0");
                setFile(null);
                setColor(0);
            }
            else if (!File.Exists(src))
            {
                output.Warning($"WARNING: Cannot find file '{Path.GetFileName(src)}' for {label} channel, the value will be set to 0");
                setFile(null);
                setColor(0);
            }
            else
            {
                setFile(src);
                output.Info($"Channel {label} set to file '{Path.GetFileName(src)}'");
                setColor(0);
            }
        }

    }
}

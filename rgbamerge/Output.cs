using System;

namespace rgbamerge
{
    public interface IOutput
    {
        void Info(string text);
        void Warning(string text);
        void Error(string text);
    }
    public sealed class Output : IOutput
    {
        void IOutput.Info(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        void IOutput.Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        void IOutput.Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
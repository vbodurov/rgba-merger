using System;
using System.IO;

namespace rgbamerge
{
    public class InnerOptions
    {
        public byte Color_R { get; set; }
        public string File_R { get; set; }
        public byte Color_G { get; set; }
        public string File_G { get; set; }
        public byte Color_B { get; set; }
        public string File_B { get; set; }
        public byte Color_A { get; set; }
        public string File_A { get; set; }
        public string File_O { get; set; }
    }
}
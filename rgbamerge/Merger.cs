using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace rgbamerge
{
    public interface IMerger
    {
        void Merge();
    }
    public sealed class Merger : IMerger
    {
        readonly IOutput _output;
        readonly InnerOptions _io;
        public Merger(IOutput output, InnerOptions io)
        {
            _output = output;
            _io = io;
        }
        void IMerger.Merge()
        {
            var srcR = (name: _io.File_R, img: CreateImege(_io.File_R), color: _io.Color_R, channel: "Red");
            var srcG = (name: _io.File_G, img: CreateImege(_io.File_G), color: _io.Color_G, channel: "Green");
            var srcB = (name: _io.File_B, img: CreateImege(_io.File_B), color: _io.Color_B, channel: "Blue");
            var srcA = (name: _io.File_A, img: CreateImege(_io.File_A), color: _io.Color_A, channel: "Alpha");
            var images = new[] {srcR, srcG, srcB, srcA};
            if (!ValidateImageSize(images)) return;

            Bitmap targ = new Bitmap(images.Select(e => e.img).First(e => e != null));
            BitmapData dataR = (srcR.img as Bitmap)?.LockBits(new Rectangle(0, 0, srcR.img.Width, srcR.img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData dataG = (srcG.img as Bitmap)?.LockBits(new Rectangle(0, 0, srcG.img.Width, srcG.img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData dataB = (srcB.img as Bitmap)?.LockBits(new Rectangle(0, 0, srcB.img.Width, srcB.img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData dataA = (srcA.img as Bitmap)?.LockBits(new Rectangle(0, 0, srcA.img.Width, srcA.img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            
            BitmapData newData = ((Bitmap) targ).LockBits(new Rectangle(0, 0, targ.Width, targ.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int stride = newData.Stride;

            System.IntPtr newScan0 = newData.Scan0;

            unsafe
            {
                byte* pointerR = null;
                byte* pointerG = null;
                byte* pointerB = null;
                byte* pointerA = null;
                if (dataR != null) pointerR = (byte*)(void*)dataR.Scan0;
                if (dataG != null) pointerG = (byte*)(void*)dataG.Scan0;
                if (dataB != null) pointerB = (byte*)(void*)dataB.Scan0;
                if (dataA != null) pointerA = (byte*)(void*)dataA.Scan0;
                byte* pNew = (byte*)(void*)newScan0;

                int nOffset = stride - targ.Width * 4;

                //                byte red, green, blue, alpha;
                byte blue, green, red, alpha;

                for (int y = 0; y < targ.Height; ++y)
                {
                    for (int x = 0; x < targ.Width; ++x)
                    {
                        // blue = pOriginalRGB[0];
                        // green = pOriginalRGB[1];
                        // red = pOriginalRGB[2];
                        // alpha = pOriginalA[0];
                        red = srcR.img == null ? _io.Color_R : pointerR[2];
                        green = srcG.img == null ? _io.Color_G : pointerG[2];
                        blue = srcB.img == null ? _io.Color_B : pointerB[2];
                        alpha = srcA.img == null ? _io.Color_A : pointerA[2];

                        pNew[0] = blue;// BLUE 
                        pNew[1] = green;// GREEN
                        pNew[2] = red;// RED
                        pNew[3] = alpha;// ALPHA

                        if (srcR.img != null) pointerR += 4;
                        if (srcG.img != null) pointerG += 4;
                        if (srcB.img != null) pointerB += 4;
                        if (srcA.img != null) pointerA += 4;
                        pNew += 4;
                    }
                    if (srcR.img != null) pointerR += nOffset;
                    if (srcG.img != null) pointerG += nOffset;
                    if (srcB.img != null) pointerB += nOffset;
                    if (srcA.img != null) pointerA += nOffset;
                    pNew += nOffset;
                }
            }
            (srcR.img as Bitmap)?.UnlockBits(dataR);
            (srcG.img as Bitmap)?.UnlockBits(dataG);
            (srcB.img as Bitmap)?.UnlockBits(dataB);
            (srcA.img as Bitmap)?.UnlockBits(dataA);
            var targBitmap = ((Bitmap) targ);
            targBitmap.UnlockBits(newData);

            if (File.Exists(_io.File_O))
            {
                File.Delete(_io.File_O);
            }
            var outDir = Path.GetDirectoryName(_io.File_O);
            if (outDir != null && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            targBitmap.Save(_io.File_O);
            _output.Info("CREATED FILE: " + _io.File_O);
        }
        static Image CreateImege(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
            return Image.FromFile(path);
        }
        bool ValidateImageSize(params (string name, Image img, byte color, string channel)[] images)
        {
            if (images == null || images.Length == 0)
            {
                _output.Error("ERROR: No image found, at least one image is required");
                return false;
            }
            var prev = images.First();
            var isFirst = true;
            foreach (var curr in images
                    .Where(e => e.img != null))
            {
                if (!isFirst)
                {
                    if (curr.img.Width != prev.img.Width)
                    {
                        _output.Error(
                            $"ERROR: Images have different width" +
                            $" {prev.name}={prev.img.Width} ({prev.channel}) and"+
                            $" {curr.name}={curr.img.Width} ({curr.channel})");
                        return false;
                    }
                    if (curr.img.Height != prev.img.Height)
                    {
                        _output.Error(
                            $"ERROR: Images have different height" +
                            $" {prev.name}={prev.img.Height} ({prev.channel}) and" +
                            $" {curr.name}={curr.img.Height} ({curr.channel})");
                        return false;
                    }
                }
                isFirst = false;
                prev = curr;
            }
            return true;
        }
    }
}
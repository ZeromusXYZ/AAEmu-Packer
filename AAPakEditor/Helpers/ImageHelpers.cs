using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace AAPakEditor.Helpers;

internal class ImageHelpers
{ 
    public static Bitmap ReadDdsFromStream(Stream stream)
    {
        using var image = Pfim.Pfimage.FromStream(stream);
        var format = image.Format switch
        {
            Pfim.ImageFormat.Rgb24 => PixelFormat.Format24bppRgb,
            Pfim.ImageFormat.Rgba32 => PixelFormat.Format32bppArgb,
            Pfim.ImageFormat.R5g5b5 => PixelFormat.Format16bppRgb555,
            Pfim.ImageFormat.R5g6b5 => PixelFormat.Format16bppRgb565,
            Pfim.ImageFormat.R5g5b5a1 => PixelFormat.Format16bppArgb1555,
            Pfim.ImageFormat.Rgb8 => PixelFormat.Format8bppIndexed,
            _ => throw new NotImplementedException()
        };

        var pointer = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
        using var bitmap = new Bitmap(image.Width, image.Height, image.Stride, format, pointer);
        var clone = new Bitmap(image.Width, image.Height, format);

        using var g = Graphics.FromImage(clone);
        g.CompositingMode = CompositingMode.SourceCopy;
        g.DrawImage(bitmap, 0, 0);

        return clone;
    }
}
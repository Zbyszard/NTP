using SkiaSharp;
using System.IO;
using System.Threading.Tasks;

namespace Kopyw.ImageProcessing.Interfaces
{
    public interface IImageReader
    {
        SKBitmap Bitmap { get; }
        SKImage Image { get; }
        SKImageInfo Info { get; }
        SKEncodedImageFormat Format { get; }
        Stream FileStream { get; }

        Task Init(Stream sourceStream);
        Task<byte[]> ReadAllBytes(string path);
    }
}
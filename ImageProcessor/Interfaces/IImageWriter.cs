using SkiaSharp;
using System.Threading.Tasks;

namespace Kopyw.ImageProcessing.Interfaces
{
    public interface IImageWriter
    {
        Task Write(SKImage image, string path, SKEncodedImageFormat format);
        Task TryClearUp(params string[] paths);
    }
}
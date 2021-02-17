using SkiaSharp;
using System.Threading.Tasks;

namespace Kopyw.ImageProcessing.Interfaces
{
    public interface IImageProcessor
    {
        Task<SKImage> GetScaledDownImage(SKBitmap bitmap, SKImageInfo imageInfo, SKFilterQuality quality, int maxDimensionLength);
    }
}
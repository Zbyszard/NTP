using Kopyw.ImageProcessing.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kopyw.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        public async Task<SKImage> GetScaledDownImage(SKBitmap bitmap, SKImageInfo imageInfo,
            SKFilterQuality quality, int maxDimensionLength)
        {
            int width = imageInfo.Width, height = imageInfo.Height;
            float scale;
            if (width >= height)
                scale = (float)maxDimensionLength / width;
            else
                scale = (float)maxDimensionLength / height;
            var newSize = new SKSizeI((int)(scale * width), (int)(scale * height));
            return await Task.Run(() => SKImage.FromBitmap(bitmap.Resize(newSize, quality)));
        }
    }
}

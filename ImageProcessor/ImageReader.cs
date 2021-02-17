using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kopyw.ImageProcessing.Interfaces;
using SkiaSharp;

namespace Kopyw.ImageProcessing
{
    public class ImageReader : IImageReader
    {
        public SKImage Image { get; private set; }
        public SKBitmap Bitmap { get; private set; }
        public SKImageInfo Info { get; private set; }
        public SKEncodedImageFormat Format { get; private set; }
        public Stream FileStream { get; private set; }

        public async Task Init(Stream sourceStream)
        {
            if (Image != null)
                return;
            FileStream = sourceStream;
            var reading = Task.Run(() =>
            {
                var data = SKData.Create(sourceStream);
                var image = SKImage.FromEncodedData(data);
                var bitmap = SKBitmap.FromImage(image);
                var codec = SKCodec.Create(data);
                SKEncodedImageFormat imgFormat = codec.EncodedFormat;
                SKImageInfo info = codec.Info;
                return (image, bitmap, info, imgFormat);
            });
            (Image, Bitmap, Info, Format) = await reading;
        }

        public async Task<byte[]> ReadAllBytes(string path)
        {
            return await Task.Run(() => File.ReadAllBytes(path));
        }
    }
}

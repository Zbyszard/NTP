using SkiaSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kopyw.ImageProcessing.Interfaces;

namespace Kopyw.ImageProcessing
{
    public class ImageWriter : IImageWriter
    {
        public async Task Write(SKImage image, string path, SKEncodedImageFormat format)
        {
            await Task.Run(() =>
            {
                using var output = File.OpenWrite(path);
                image.Encode(format, 80).SaveTo(output);
            });
        }

        public async Task TryClearUp(params string[] paths)
        {
            await Task.Run(() =>
            {
                foreach (var path in paths)
                {
                    try
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                    catch (IOException)
                    {
                        continue;
                    }
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Options
{
    public class ImageManagerOptions
    {
        public const string ImageManager = "ImageManager";

        public string MiniaturePrefix { get; set; }
        public string PostImagePath { get; set; }
        public string MessageImagePath { get; set; }
        public int MaxMiniatureDimensionPx { get; set; }
        public string MiniatureQuality { get; set; }
    }
}

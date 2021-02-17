using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Kopyw.ImageProcessing.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.Collections.Generic;
using Kopyw.Core.Repositiories;
using Kopyw.Infrastructure.Options;
using Kopyw.Core.Models;

namespace Kopyw.Infrastructure.Repositories
{
    public class FileSystemImageManager : IFileSystemImageManager
    {
        private readonly ImageManagerOptions options;
        private readonly ILogger<FileSystemImageManager> logger;
        private readonly IImageReader reader;
        private readonly IImageProcessor processor;
        private readonly IImageWriter writer;

        public Stream FileStream { get; private set; }

        public FileSystemImageManager(IOptions<ImageManagerOptions> options, 
            ILogger<FileSystemImageManager> logger,
            IImageReader imageReader,
            IImageProcessor imageProcessor,
            IImageWriter imageWriter)
        {
            this.options = options.Value;
            this.logger = logger;
            reader = imageReader;
            processor = imageProcessor;
            writer = imageWriter;
        }

        private void EnsureDirectoriesExists()
        {
            try
            {
                if (!Directory.Exists(options.PostImagePath))
                    Directory.CreateDirectory(options.PostImagePath);

                if (!Directory.Exists(options.MessageImagePath))
                    Directory.CreateDirectory(options.MessageImagePath);
            }
            catch (Exception e)
            {
                logger.LogError("Could not initialize image directories: {error}", e);
                throw;
            }
        }

        public async Task<byte[]> GetImageBytes(string path)
        {
            return await reader.ReadAllBytes(path);
        }

        public async Task<ImageInfo> SavePostImage(IFormFile file)
        {
            var info = await ReadAndSave(file, options.PostImagePath);
            return info;
        }

        public async Task<ImageInfo> SaveMessageImage(IFormFile file)
        {
            var info = await ReadAndSave(file, options.MessageImagePath);
            info.IsPrivate = true;
            return info;
        }


        private async Task<ImageInfo> ReadAndSave(IFormFile file, string directory)
        {
            EnsureDirectoriesExists();
            using (var stream = file.OpenReadStream())
                await reader.Init(stream);

            var miniature = await CreateMiniature();
            var info = await SaveImages(reader.Image, miniature, directory);
            
            return info;
        }

        private async Task<SKImage> CreateMiniature()
        {
            var quality = Enum.Parse<SKFilterQuality>(options.MiniatureQuality); 
            int max = options.MaxMiniatureDimensionPx;
            if (reader.Info.Height < max && reader.Info.Width < max)
                return null;
            return await processor.GetScaledDownImage(reader.Bitmap, reader.Info, quality, max);
        }

        private async Task<ImageInfo> SaveImages (SKImage image, SKImage miniature, string directory)
        {
            string formatStr = reader.Format == SKEncodedImageFormat.Jpeg ? "jpg" : reader.Format.ToString().ToLower();
            string id = GenerateId();
            string path = $"{directory}/{id}.{formatStr}";

            var tasks = new List<Task>() { writer.Write(image, path, reader.Format) };

            string minipath;
            if (miniature != null)
            {
                minipath = $"{directory}/{options.MiniaturePrefix}{id}.{formatStr}";
                tasks.Add(writer.Write(miniature, minipath, reader.Format));
            }
            else
                minipath = path;

            try
            {
                await Task.WhenAll(tasks);
            }
            catch
            {
                await writer.TryClearUp(path, minipath);
                throw;
            }

            return new ImageInfo { Id = id, Path = path, MiniaturePath = minipath, Format = formatStr };
        }

        private string GenerateId()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            string b64 = Convert.ToBase64String(bytes);
            var reg = new Regex("[^A-Za-z0-9]");
            return reg.Replace(b64, string.Empty);
        }
    }
}

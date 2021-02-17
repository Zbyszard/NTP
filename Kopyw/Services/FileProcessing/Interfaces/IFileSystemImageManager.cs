using Kopyw.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.FileProcessing.Interfaces
{
    public interface IFileSystemImageManager
    {
        Task<byte[]> GetImageBytes(string path);
        Task<ImageInfo> SavePostImage(IFormFile file);

        Task<ImageInfo> SaveMessageImage(IFormFile file);
    }
}

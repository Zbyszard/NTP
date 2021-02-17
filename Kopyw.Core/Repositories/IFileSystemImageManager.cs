using Kopyw.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IFileSystemImageManager
    {
        Task<byte[]> GetImageBytes(string path);
        Task<ImageInfo> SavePostImage(IFormFile file);

        Task<ImageInfo> SaveMessageImage(IFormFile file);
    }
}

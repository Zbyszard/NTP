using Kopyw.Core.Models;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IImageManager
    {
        Task<ImageInfo> Get(string id);
        Task Add(ImageInfo info);
        Task Delete(string id);
    }
}

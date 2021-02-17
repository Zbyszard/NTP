using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IImageManager
    {
        Task<ImageInfo> Get(string id);
        Task Add(ImageInfo info);
        Task Delete(string id);
    }
}

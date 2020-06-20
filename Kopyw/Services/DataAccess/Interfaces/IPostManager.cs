using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IPostManager
    {
        public Task<Post> Add(Post newPost);
        public Task<Post> Get(long id);
        public Task Update(Post post);
        public Task<bool?> Delete(long id, string loggedUserId);
    }
}

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
        public Task<List<Post>> GetRange(int count, int offset, string sort);
        public Task<List<Post>> GetUserPosts(string userName, int count, int offset, string sort);
        public Task<List<Post>> GetFollowedPosts(string followedUserName, int count, int offset, string sort);
        public Task<Post> Update(Post post);
        public Task<Post> Delete(long id);
    }
}

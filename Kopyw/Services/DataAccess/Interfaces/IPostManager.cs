using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IPostManager
    {
        public Post Add(Post newPost);
        public Post Get(long id);
        public List<Post> GetRange(int count, int offset, string sort);
        public List<Post> GetUserPosts(string userName, int count, int offset, string sort);
        public List<Post> GetFollowedPosts(string followedUserName, int count, int offset, string sort);
        public Post Update(Post post);
        public Post Delete(long id);
    }
}

using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IPostDTOManager
    {
        public PostDTO Add(PostDTO newPost);
        public PostDTO Get(long id);
        public List<PostDTO> GetRange(int count, int offset, string sort);
        public List<PostDTO> GetUserPosts(string userName, int count, int offset, string sort);
        public List<PostDTO> GetFollowedPosts(string followedUserName, int count, int offset, string sort);
        public PostDTO Update(PostDTO post);
        public PostDTO Delete(long id);
    }
}

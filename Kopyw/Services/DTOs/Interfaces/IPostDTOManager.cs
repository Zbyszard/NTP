using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IPostDTOManager
    {
        public Task<PostDTO> Add(PostDTO newPost);
        public Task<PostDTO> Get(long id, string loggedUserId);
        public Task<List<PostDTO>> GetRange(int count, int offset, string sort, string loggedUserId);
        public Task<List<PostDTO>> GetUserPosts(string userName, int count, int offset, string sort);
        public Task<List<PostDTO>> GetFollowedPosts(string followedUserName, int count, int offset, string sort, string loggedUserId);
        public Task<PostDTO> Update(PostDTO post);
        public Task<PostDTO> Delete(long id);
    }
}

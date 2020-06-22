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
        public Task<List<PostDTO>> GetRange(int count, int page, string loggedUserId, string sort, string sortDir);
        public int GetPages(int postsPerPage);
        public Task<List<PostDTO>> GetUserPosts(int count, int page, string userName, string loggedUserId, string sort, string sortDir);
        public int GetUserPages(string userName, int postsPerPage);
        public Task<List<PostDTO>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortDir);
        public int GetFollowedPages(string loggedUserId, int postsPerPage);
        public Task<bool?> Update(PostDTO post, string loggedUserId);
        public Task<PostVoteDTO> AddVote(PostVoteDTO newVoteDTO);
        public Task<PostVoteDTO> DeleteVote(PostVoteDTO voteDTO);
    }
}

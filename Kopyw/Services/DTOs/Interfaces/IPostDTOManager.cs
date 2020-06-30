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
        public Task<PostDTO> Get(long id);
        public Task<List<PostDTO>> GetPage(int count, int page, string sort, string sortOrder);
        public int GetPagesCount(int postsPerPage);
        public Task<List<PostDTO>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder);
        public int GetUserPagesCount(string userName, int postsPerPage);
        public Task<List<PostDTO>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder);
        public int GetFollowedPagesCount(string loggedUserId, int postsPerPage);
        public Task<List<PostDTO>> Search(string phrase, int count, int page, string sort, string sortOrder);
        public int GetSearchPagesCount(string phrase, int postsPerPage);
        public Task<List<PostInfoDTO>> GetInformation(List<long> ids, string loggedUserId);
        public Task<bool?> Update(PostDTO post, string loggedUserId);
        public Task<PostVoteDTO> AddVote(PostVoteDTO newVoteDTO);
        public Task<PostVoteDTO> DeleteVote(PostVoteDTO voteDTO);
    }
}

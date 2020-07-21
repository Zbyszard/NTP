using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IPostDTOManager
    {
       Task<PostDTO> Add(PostDTO newPost);
       Task<PostDTO> Get(long id);
       Task<List<PostDTO>> GetPage(int count, int page, string sort, string sortOrder);
       int GetPagesCount(int postsPerPage);
       Task<List<PostDTO>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder);
       int GetUserPagesCount(string userName, int postsPerPage);
       Task<List<PostDTO>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder);
       int GetFollowedPagesCount(string loggedUserId, int postsPerPage);
       Task<List<PostDTO>> Search(string phrase, int count, int page, string sort, string sortOrder);
       int GetSearchPagesCount(string phrase, int postsPerPage);
       Task<List<PostInfoDTO>> GetInformation(List<long> ids, string loggedUserId);
       Task<PostDTO> Update(PostDTO post);
       Task<PostVoteDTO> AddVote(PostVoteDTO newVoteDTO);
       Task<PostVoteDTO> DeleteVote(PostVoteDTO voteDTO);
    }
}

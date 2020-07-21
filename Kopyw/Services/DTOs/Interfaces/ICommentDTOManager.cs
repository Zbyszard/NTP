using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface ICommentDTOManager
    {
        Task<CommentDTO> Add(CommentDTO newComment);
        Task<CommentDTO> Get(long id);
        Task<List<CommentDTO>> GetPage(long postId, string userId);
        Task<CommentDTO> Update(CommentDTO comment);
        Task<CommentDTO> Delete(long id, string loggedUserId);
        Task<CommentVoteDTO> Vote(CommentVoteDTO vote);
        Task<CommentVoteDTO> DeleteVote(CommentVoteDTO vote);
    }
}

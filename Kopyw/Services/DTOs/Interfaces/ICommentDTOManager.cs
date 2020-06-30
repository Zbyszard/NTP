using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface ICommentDTOManager
    {
        public Task<CommentDTO> Add(CommentDTO newComment);
        public Task<CommentDTO> Get(long id);
        public Task<List<CommentDTO>> GetPage(long postId, string userId);
        public Task<CommentDTO> Update(CommentDTO Comment);
        public Task<CommentDTO> Delete(long id);
        public Task<CommentVoteDTO> Vote(CommentVoteDTO vote);
        public Task<CommentVoteDTO> DeleteVote(CommentVoteDTO vote);
    }
}

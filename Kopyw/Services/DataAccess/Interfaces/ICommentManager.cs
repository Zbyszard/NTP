using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface ICommentManager
    {
        public Task<Comment> Add(Comment newComment);
        public Task<Comment> Get(long id);
        public Task<List<Comment>> GetPage(long postId);
        public Task<Comment> Update(Comment comment);
        public Task<List<CommentVote>> GetVotes(List<Comment> comments, string userId);
        public Task<CommentVote> Vote(CommentVote vote);
        public Task<CommentVote> DeleteVote(CommentVote vote);
        public Task<Comment> Delete(long id);
    }
}

using Kopyw.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface ICommentManager
    {
        Task<Comment> Add(Comment newComment);
        Task<Comment> Get(long id);
        Task<List<Comment>> GetPage(long postId);
        Task<Comment> Update(Comment comment);
        List<CommentVote> GetVotes(List<Comment> comments, string userId);
        Task<CommentVote> Vote(CommentVote vote);
        Task<CommentVote> DeleteVote(CommentVote vote);
        Task<Comment> Delete(long id, string loggedUserId);
    }
}

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
        public Task<List<Comment>> GetRange(long postId);
        public Task<Comment> Update(Comment comment);
        public Task<Comment> Delete(long id);
    }
}

using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface ICommentManager
    {
        public Task<Comment> Add(Post newComment);
        public Task<Comment> Get(long id);
        public Task<List<Comment>> GetRange(int count, int offset, string sort);
        public Task<Comment> Update(Comment comment);
        public Task<Comment> Delete(long id);
    }
}

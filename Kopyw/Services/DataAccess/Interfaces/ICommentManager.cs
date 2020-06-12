using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface ICommentManager
    {
        public Comment Add(Post newComment);
        public Comment Get(long id);
        public List<Comment> GetRange(int count, int offset, string sort);
        public Comment Update(Comment comment);
        public Comment Delete(long id);
    }
}

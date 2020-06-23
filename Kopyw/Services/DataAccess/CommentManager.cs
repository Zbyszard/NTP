using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class CommentManager : ICommentManager
    {
        private readonly ApplicationDbContext db;
        public CommentManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        private IQueryable<Comment> CommentQuery()
        {
            var query = (from c in db.Comments
                         select c)
                         .Include(c => c.Author)
                         .Include(c => c.Votes);
            return query;
        }
        public async Task<Comment> Add(Comment newComment)
        {
            db.Entry(newComment).State = EntityState.Added;
            if (await db.SaveChangesAsync() == 1)
                return newComment;
            else
                return null;
        }

        public async Task<Comment> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> Get(long id)
        {
            var comment = await CommentQuery().Where(c => c.Id == id).FirstOrDefaultAsync();
            return comment;
        }

        public async Task<List<Comment>> GetRange(long postId)
        {
            var comments = await CommentQuery().Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }

        public async Task<Comment> Update(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}

using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task<List<Comment>> GetPage(long postId)
        {
            var comments = await CommentQuery().Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }

        public async  Task<Comment> Update(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CommentVote>> GetVotes(List<Comment> comments, string userId)
        {
            var votes = (from c in comments
                         select new CommentVote
                         {
                             CommentId = c.Id,
                             UserId = userId,
                             Value = (from cv in db.CommentVotes
                                      where cv.CommentId == c.Id && cv.UserId == userId
                                      select cv.Value).FirstOrDefault()
                         }).ToList();
            return votes;
        }

        public async Task<CommentVote> Vote(CommentVote vote)
        {
            var oldVote = await db.CommentVotes
                .Where(c => c.UserId == vote.UserId && c.CommentId == vote.CommentId)
                .FirstOrDefaultAsync();
            if (oldVote != null)
            {
                oldVote.Value = vote.Value;
                db.Entry(oldVote).State = EntityState.Modified;
                vote = oldVote;
            }
            else
                db.Entry(vote).State = EntityState.Added;
            if (await db.SaveChangesAsync() == 1)
                return vote;
            return null;
        }
        public async Task<CommentVote> DeleteVote(CommentVote vote)
        {
            var oldVote = await db.CommentVotes
                .Where(c => c.UserId == vote.UserId && c.CommentId == vote.CommentId)
                .FirstOrDefaultAsync();
            if(oldVote != null)
            {
                vote = oldVote;
                db.Entry(vote).State = EntityState.Deleted;
                if (await db.SaveChangesAsync() == 0)
                    return null;
                return vote;
            }
            return null;
        }
    }
}

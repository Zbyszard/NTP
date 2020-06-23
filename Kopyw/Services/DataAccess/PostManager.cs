using IdentityServer4.Extensions;
using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class PostManager : IPostManager
    {
        private readonly ApplicationDbContext db;
        public PostManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }          
        public async Task<Post> Add(Post newPost)
        {

            db.Add(newPost);
            if (await db.SaveChangesAsync() == 1)
                return newPost;
            else
                return null;
        }

        public async Task<bool?> Delete(long id, string loggedUserId)
        {
            var post = await (from p in db.Posts
                              where p.Id == id
                              select p).FirstOrDefaultAsync();
            if (post == null)
                return null;
            if (post.AuthorId != loggedUserId)
                return false;
            db.Entry(post).State = EntityState.Deleted;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<Post> Get(long id)
        {
            var post = await db.Posts.Select(p => p).Where(p => p.Id == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<int> Update(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            return await db.SaveChangesAsync();
        }

        public async Task<PostVote> AddVote(PostVote newVote)
        {
            var pv = await db.PostVotes.Where(p => p.PostId == newVote.PostId &&
                p.UserId == newVote.UserId).FirstOrDefaultAsync();
            var isSelfVote = await (from p in db.Posts
                                    where p.Id == newVote.PostId && p.AuthorId == newVote.UserId
                                    select p).CountAsync() == 1;
            if(pv == null && newVote.PostId != 0 && !newVote.UserId.IsNullOrEmpty() && !isSelfVote)
            {
                db.PostVotes.Add(newVote);
                await db.SaveChangesAsync();
                return newVote;
            }
            return null;
        }
        public async Task<PostVote> DeleteVote(PostVote vote)
        {
            var pv = await db.PostVotes.Where(p => p.PostId == vote.PostId &&
                p.UserId == vote.UserId).FirstOrDefaultAsync();
            if (pv == null)
                return null;
            db.Entry(pv).State = EntityState.Deleted;
            if (await db.SaveChangesAsync() != 1)
                return null;
            return pv;
        }
    }
}

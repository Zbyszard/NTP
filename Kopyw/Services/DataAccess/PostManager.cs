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
    public class PostManager : IPostManager
    {
        private readonly ApplicationDbContext db;
        public PostManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task<Post> Add(Post newPost)
        {
            try
            {
                db.Add(newPost);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return newPost;
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

        public async Task Update(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}

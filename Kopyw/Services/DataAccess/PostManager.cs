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

        public async Task<Post> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> GetFollowedPosts(string followedUserName, int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> GetRange(int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> GetUserPosts(string userName, int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> Update(Post post)
        {
            throw new NotImplementedException();
        }
    }
}

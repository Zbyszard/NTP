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
    public class FollowManager : IFollowManager
    {
        private readonly ApplicationDbContext db;
        public FollowManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task<Follow> Add(Follow newFollow)
        {
            var old = await (from f in db.Follows
                             where f.AuthorId == newFollow.AuthorId && f.ObserverId == newFollow.ObserverId
                             select f).FirstOrDefaultAsync();
            if (old != null)
                return old;
            db.Entry(newFollow).State = EntityState.Added;
            try
            {
                await db.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                return null;
            }
            return newFollow;
        }

        public async Task<Follow> Delete(string authorId, string userId)
        {
            var follow = await (from f in db.Follows
                                where f.Author.Id == authorId && f.Observer.Id == userId
                                select f).FirstOrDefaultAsync();
            if (follow == null)
                return null;
            db.Entry(follow).State = EntityState.Deleted;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return follow;
        }
    }
}

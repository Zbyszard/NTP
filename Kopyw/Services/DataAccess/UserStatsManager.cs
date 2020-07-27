using System;
using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class UserStatsManager : IUserStatsManager
    {
        private readonly ApplicationDbContext db;
        public UserStatsManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task<UserStats> Get(string userName, string loggedUserId)
        {
            var stats = await (from u in db.Users
                               where u.UserName == userName
                               select new UserStats
                               {
                                   User = u,
                                   LoggedUserFollow = u.FollowedBy.Where(f => f.ObserverId == loggedUserId).FirstOrDefault(),
                                   PostCount = u.Posts.Count(),
                                   CommentCount = u.Comments.Count(),
                                   PointsFromPosts = u.Posts.SelectMany(p => p.Votes).Count(),
                                   PointsFromComments = u.Comments.SelectMany(c => c.Votes).Where(cv => cv.Value > 0).Count() -
                                        u.Comments.SelectMany(c => c.Votes).Where(cv => cv.Value < 0).Count()
                               }).FirstOrDefaultAsync();
            return stats;
        }
    }
}

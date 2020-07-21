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
                                   CommentCount = u.Comments.Count()
                               }).FirstOrDefaultAsync();

            if (stats == null)
                return null;

            var postPoints = await (from pv in db.PostVotes
                                    join p in db.Posts on pv.PostId equals p.Id
                                    join u in db.Users on p.AuthorId equals u.Id
                                    where u.UserName == userName
                                    select pv).CountAsync();

            var commentVotes = from cv in db.CommentVotes
                               join c in db.Comments on cv.CommentId equals c.Id
                               join u in db.Users on c.AuthorId equals u.Id
                               where u.UserName == userName
                               select cv;

            var commentPoints = await commentVotes.Where(cv => cv.Value > 0).CountAsync()
                - await commentVotes.Where(cv => cv.Value < 0).CountAsync();
            stats.PointsFromPosts = postPoints;
            stats.PointsFromComments = commentPoints;
            return stats;
        }
    }
}

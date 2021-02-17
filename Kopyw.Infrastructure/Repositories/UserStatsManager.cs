using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.Core.Repositiories;
using Kopyw.Infrastructure.DataAccess;
using Kopyw.Core.Models;

namespace Kopyw.Infrastructure.Repositories
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

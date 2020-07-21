using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class UserStatsDTO
    {
        public string UserName { get; set; }
        public bool IsFollowed { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int PointsFromPosts { get; set; }
        public int PointsFromComments { get; set; }
    }
}

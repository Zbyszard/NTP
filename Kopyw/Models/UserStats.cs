using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    [NotMapped]
    public class UserStats
    {
        public ApplicationUser User { get; set; }
        public Follow LoggedUserFollow { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int PointsFromPosts { get; set; }
        public int PointsFromComments { get; set; }
    }
}

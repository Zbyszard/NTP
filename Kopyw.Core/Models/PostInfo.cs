using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    [NotMapped]
    public class PostInfo
    {
        public long PostId { get; set; }
        public int CommentCount { get; set; }
        public int Score { get; set; }
        public bool FollowingAuthor { get; set; }
        public PostVote UserVote { get; set; }
    }
}

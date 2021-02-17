using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.DTO
{
    public class PostInfoDTO
    {
        public long PostId { get; set; }
        public int CommentCount { get; set; }
        public int Score { get; set; }
        public bool UserVote { get; set; }
        public bool FollowingAuthor { get; set; }
    }
}

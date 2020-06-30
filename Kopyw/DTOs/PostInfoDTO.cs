using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class PostInfoDTO
    {
        public long PostId { get; set; }
        public int CommentCount { get; set; }
        public int Score { get; set; }
        public bool UserVote { get; set; }
    }
}

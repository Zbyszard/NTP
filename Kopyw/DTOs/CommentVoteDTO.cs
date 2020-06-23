using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class CommentVoteDTO
    {
        public long CommentId { get; set; }
        public string UserId { get; set; }
        public int Value { get; set; }
    }
}

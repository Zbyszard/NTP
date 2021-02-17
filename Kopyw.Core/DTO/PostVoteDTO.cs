using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.DTO
{
    public class PostVoteDTO
    {
        public long PostId { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
    }
}

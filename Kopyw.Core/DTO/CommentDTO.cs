﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.DTO
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateTime PostTime { get; set; }
        public int Score { get; set; }
        public int UserVote { get; set; }
        public bool Deleted { get; set; }
    }
}

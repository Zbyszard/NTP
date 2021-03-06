﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Kopyw.Core.DTO
{
    public class PostDTO
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }
    }
}

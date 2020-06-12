using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public int Score { get; set; }
        public int UserVote { get; set; }
    }
}

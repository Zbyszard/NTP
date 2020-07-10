using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class Post
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string AuthorId { get; set; }

        public List<Comment> Comments { get; set; }
        public List<PostVote> Votes { get; set; }
        public ApplicationUser Author { get; set; }
    }
}

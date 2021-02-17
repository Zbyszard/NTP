using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PostTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string AuthorId { get; set; }
        [Required]
        public long PostId { get; set; }
        public bool Deleted { get; set; }


        public ApplicationUser Author { get; set; }
        public Post Post { get; set; }
        public List<CommentVote> Votes { get; set; }
    }
}

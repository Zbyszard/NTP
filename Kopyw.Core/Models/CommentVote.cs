using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    public class CommentVote
    {
        public long Id { get; set; }
        [Required]
        public int Value { get; set; }
        public string UserId { get; set; }
        [Required]
        public long CommentId { get; set; }


        public ApplicationUser User { get; set; }
        public Comment Comment { get; set; }
    }
}

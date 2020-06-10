using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class PostVote
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public long PostId { get; set; }


        public ApplicationUser User { get; set; }
        public Post Post { get; set; }
    }
}

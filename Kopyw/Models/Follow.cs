using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class Follow
    {
        public long Id { get; set; }
        public string ObserverId { get; set; }
        public string AuthorId { get; set; }

        public ApplicationUser Observer { get; set; }
        public ApplicationUser Author { get; set; }
    }
}

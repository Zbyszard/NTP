using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    public class PostImage
    {
        [Key]
        public long Id { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        [Required]
        public string ImageId { get; set; }
        public ImageInfo Image { get; set; }
    }
}

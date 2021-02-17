using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    public class MessageImage
    {
        [Key]
        public long Id { get; set; }
        public long MessageId { get; set; }
        public Message Message { get; set; }
        [Required]
        public string ImageId { get; set; }
        public ImageInfo Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class ImageInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string MiniaturePath { get; set; }
        [Required]
        public string Format { get; set; }
        public bool IsPrivate { get; set; }
        public long? PostImageId { get; set; }
        public PostImage PostImage { get; set; }
        public long? MessageImageId { get; set; }
        public MessageImage MessageImage { get; set; }
    }
}

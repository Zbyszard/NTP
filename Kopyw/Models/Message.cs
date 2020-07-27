using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public long ConversationId { get; set; }
        [Required]
        public Conversation Conversation { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
    }
}

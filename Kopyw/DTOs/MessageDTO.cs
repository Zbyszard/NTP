using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class MessageDTO
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public string Sender { get; set; }
        public DateTime SendTime { get; set; }
        public string Text { get; set; }
    }
}

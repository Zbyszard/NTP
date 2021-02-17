using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.DTO
{
    public class MessageDTO
    {
        public long ConversationId { get; set; }
        public string Sender { get; set; }
        public DateTime SendTime { get; set; }
        public string Text { get; set; }
    }
}

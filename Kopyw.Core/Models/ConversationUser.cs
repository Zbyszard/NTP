using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.Models
{
    public class ConversationUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}

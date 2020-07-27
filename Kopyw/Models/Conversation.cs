using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Models
{
    public class Conversation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
        public List<ConversationUser> Participations { get; set; }
    }
}

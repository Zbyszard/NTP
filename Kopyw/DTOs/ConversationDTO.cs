using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.DTOs
{
    public class ConversationDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<string> Users { get; set; }
    }
}

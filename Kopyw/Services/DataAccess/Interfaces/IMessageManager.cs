using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IConversationManager
    {
        Task<Conversation> AddConversation(Conversation conversation);
        Task<List<Conversation>> GetConversations(string userId, int count, DateTime? olderThan);
        Task<Message> AddMessage(Message message);
        Task<List<Message>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan);
    }
}

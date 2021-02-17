using Kopyw.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IConversationManager
    {
        Task<Conversation> AddConversation(Conversation conversation);
        Task<List<Conversation>> GetConversations(string userId, int count, DateTime? olderThan);
        Task<Conversation> GetConversation(long id);
        Task<List<Conversation>> SearchConversations(string searchString, string loggedUserName);
        Task<Message> AddMessage(Message message);
        Task<List<Message>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan);
    }
}

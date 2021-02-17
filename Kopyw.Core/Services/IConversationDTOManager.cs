using Kopyw.Core.DTO;
using Kopyw.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kopyw.Core.Services
{
    public interface IConversationDTOManager
    {
        Task<ConversationDTO> AddConversation(ConversationDTO conversation, ApplicationUser invokingUser);
        Task<List<ConversationDTO>> GetConversations(string userId, int count, DateTime? olderThan);
        Task<ConversationDTO> GetConversation(long id);
        Task<List<ConversationDTO>> SearchConversations(string searchString, string loggedUserName);
        Task<MessageDTO> AddMessage(MessageDTO message, ApplicationUser sender);
        Task<List<MessageDTO>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan);
    }
}

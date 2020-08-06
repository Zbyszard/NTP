using Kopyw.DTOs;
using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IConversationDTOManager
    {
        Task<ConversationDTO> AddConversation(ConversationDTO conversation, ApplicationUser invokingUser);
        Task<List<ConversationDTO>> GetConversations(string userId, int count, DateTime? olderThan);
        Task<ConversationDTO> GetConversation(long id);
        Task<MessageDTO> AddMessage(MessageDTO message, ApplicationUser sender);
        Task<List<MessageDTO>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan);
    }
}

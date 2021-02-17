using AutoMapper;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs
{
    public class ConversationDTOManager : IConversationDTOManager
    {
        private readonly IConversationManager conversationManager;
        private readonly IMapper mapper;
        private readonly IUserFinder userFinder;
        
        public ConversationDTOManager(IConversationManager conversationManager,
            IMapper mapper,
            IUserFinder userFinder)
        {
            this.conversationManager = conversationManager;
            this.mapper = mapper;
            this.userFinder = userFinder;
        }
        public async Task<ConversationDTO> AddConversation(ConversationDTO conversation, ApplicationUser invokingUser)
        {
            if (invokingUser == null)
                return null;
            var users = await userFinder.FindUsersByNames(conversation.UserNames);
            if(!users.Any(u => u.Id == invokingUser.Id))
                users.Add(invokingUser);
            var dbConversation = new Conversation
            {
                IsGroup = conversation.IsGroup,
                Participations = users.Select(u => new ConversationUser { User = u }).ToList()
            };
            foreach (var convUser in dbConversation.Participations)
                convUser.Conversation = dbConversation;
            
            var added = await conversationManager.AddConversation(dbConversation);
            return mapper.Map<ConversationDTO>(added);
        }

        public async Task<List<ConversationDTO>> GetConversations(string userId, int count, DateTime? olderThan)
        {
            var conversations = await conversationManager.GetConversations(userId, count, olderThan);
            return mapper.Map<List<ConversationDTO>>(conversations);
        }

        public async Task<ConversationDTO> GetConversation(long id)
        {
            var conv = await conversationManager.GetConversation(id);
            return mapper.Map<ConversationDTO>(conv);
        }

        public async Task<List<ConversationDTO>> SearchConversations(string searchString, string loggedUserName)
        {
            var convs = await conversationManager.SearchConversations(searchString, loggedUserName);
            return mapper.Map<List<ConversationDTO>>(convs);
        }
        public async Task<MessageDTO> AddMessage(MessageDTO message, ApplicationUser sender)
        {
            var dbMessage = mapper.Map<Message>(message);
            dbMessage.Sender = sender;
            var added = await conversationManager.AddMessage(dbMessage);
            return mapper.Map<MessageDTO>(added);
        }

        public async Task<List<MessageDTO>> GetMessages(long conversationId, string userId, int count, DateTime? olderThan)
        {
            var messages = await conversationManager.GetMessages(conversationId, userId, count, olderThan);
            return mapper.Map<List<MessageDTO>>(messages);
        }
    }
}

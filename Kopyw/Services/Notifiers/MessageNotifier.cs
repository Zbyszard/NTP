using Kopyw.DTOs;
using Kopyw.Hubs;
using Kopyw.Hubs.ClientInterfaces;
using Kopyw.Services.DTOs.Interfaces;
using Kopyw.Services.Notifiers.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.Notifiers
{
    public class MessageNotifier : IMessageNotifier
    {
        private readonly IHubContext<MessageHub, IMessageHubClient> hubContext;
        private readonly IConversationDTOManager conversationManager;
        private readonly IUserFinder userFinder;
        public MessageNotifier(IHubContext<MessageHub, IMessageHubClient> hubContext,
            IConversationDTOManager conversationManager,
            IUserFinder userFinder)
        {
            this.hubContext = hubContext;
            this.conversationManager = conversationManager;
            this.userFinder = userFinder;
        }
        public async Task SendMessage(MessageDTO message)
        {
            var conv = await conversationManager.GetConversation(message.ConversationId);
            var ids = await userFinder.FindIdsByNames(conv.UserNames);
            await hubContext.Clients.Users(ids).MessageReceived(message);
        }
    }
}

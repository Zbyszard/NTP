using Kopyw.DTOs;
using Kopyw.Hubs;
using Kopyw.Hubs.ClientInterfaces;
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
        public MessageNotifier(IHubContext<MessageHub, IMessageHubClient> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task SendMessage(MessageDTO message)
        {
            await hubContext.Clients.Group($"{message.ConversationId}").ReceiveMessage(message);
        }
    }
}

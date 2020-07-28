using Kopyw.Hubs.ClientInterfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs
{
    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task Subscrbe(long conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{conversationId}");
        }
        public async Task Unsubscrbe(long conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{conversationId}");
        }
    }
}

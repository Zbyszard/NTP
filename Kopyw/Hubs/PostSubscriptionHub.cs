using Kopyw.DTOs;
using Kopyw.Hubs.ClientInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs
{
    public class PostSubscriptionHub : Hub<IPostSubscriptionHubClient>
    {
        public async Task Subscribe(long postId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Post {postId}");
        }

        public async Task Unsubscribe(long postId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Post {postId}");
        }
    }
}

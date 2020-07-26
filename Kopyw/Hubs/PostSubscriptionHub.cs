using Kopyw.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs
{
    public class PostSubscriptionHub : Hub
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

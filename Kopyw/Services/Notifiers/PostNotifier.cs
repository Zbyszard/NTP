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
    public class PostNotifier : IPostNotifier
    {
        private readonly IPostDTOManager postManager;
        private readonly IHubContext<PostSubscriptionHub, IPostSubscriptionHubClient> hubContext;
        public PostNotifier(IPostDTOManager postManager,
            IHubContext<PostSubscriptionHub, IPostSubscriptionHubClient> hubContext)
        {
            this.postManager = postManager;
            this.hubContext = hubContext;
        }
        public async Task SendUpdate(long postId)
        {
            var update = await postManager.GetUpdate(postId);
            if (update == null)
                return;
            await hubContext.Clients.Group($"Post {postId}").UpdateReceived(update);
        }
    }
}

using Kopyw.Core.HubClients;
using Kopyw.Core.Notifications;
using Kopyw.Core.Services;
using Kopyw.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Notifications
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

using Kopyw.Core.DTO;
using Kopyw.Core.HubClients;
using Kopyw.Core.Notifications;
using Kopyw.Core.Services;
using Kopyw.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Notifications
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

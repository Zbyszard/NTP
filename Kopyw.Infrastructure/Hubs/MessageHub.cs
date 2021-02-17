using Kopyw.Core.HubClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Hubs
{
    [Authorize]
    public class MessageHub : Hub<IMessageHubClient>
    {

    }
}

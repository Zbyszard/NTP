using Kopyw.Hubs.ClientInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs
{
    [Authorize]
    public class MessageHub : Hub<IMessageHubClient>
    {

    }
}

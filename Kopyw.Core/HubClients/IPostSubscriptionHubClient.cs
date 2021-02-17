using Kopyw.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Core.HubClients
{
    public interface IPostSubscriptionHubClient
    {
        Task UpdateReceived(PostInfoDTO update);
    }
}

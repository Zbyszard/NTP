using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs.ClientInterfaces
{
    public interface IMessageClient
    {
        Task ReceiveMessage(long conversationId);
    }
}

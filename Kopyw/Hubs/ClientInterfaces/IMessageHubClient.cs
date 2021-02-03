﻿using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Hubs.ClientInterfaces
{
    public interface IMessageHubClient
    {
        Task MessageReceived (MessageDTO message);
    }
}

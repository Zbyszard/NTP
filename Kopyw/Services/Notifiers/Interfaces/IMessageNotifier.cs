using Kopyw.DTOs;
using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.Notifiers.Interfaces
{
    public interface IMessageNotifier
    {
        Task SendMessage(MessageDTO message);
    }
}

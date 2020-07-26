using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.Notifiers.Interfaces
{
    public interface IPostNotifier
    {
        Task SendUpdate(long postId);
    }
}

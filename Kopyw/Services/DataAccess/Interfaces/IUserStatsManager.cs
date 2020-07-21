using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IUserStatsManager
    {
        Task<UserStats> Get(string userName, string loggedUserId);
    }
}

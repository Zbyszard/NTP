using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IUserStatsDTOManager
    {
        Task<UserStatsDTO> Get(string userName, string loggedUserId);
    }
}

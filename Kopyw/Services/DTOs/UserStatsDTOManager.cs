using AutoMapper;
using Kopyw.DTOs;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs
{
    public class UserStatsDTOManager : IUserStatsDTOManager
    {
        private readonly IUserStatsManager statsManager;
        private readonly IMapper mapper;
        public UserStatsDTOManager(IUserStatsManager statsManager,
            IMapper mapper)
        {
            this.statsManager = statsManager;
            this.mapper = mapper;
        }
        public async Task<UserStatsDTO> Get(string userName, string loggedUserId)
        {
            var stats = await statsManager.Get(userName, loggedUserId);
            var dto = mapper.Map<UserStatsDTO>(stats);
            return dto;
        }
    }
}

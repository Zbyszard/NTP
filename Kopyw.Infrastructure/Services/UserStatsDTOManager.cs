using AutoMapper;
using Kopyw.Core.DTO;
using Kopyw.Core.Repositiories;
using Kopyw.Core.Services;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Services
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

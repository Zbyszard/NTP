using Kopyw.Core.DTO;
using System.Threading.Tasks;

namespace Kopyw.Core.Services
{
    public interface IUserStatsDTOManager
    {
        Task<UserStatsDTO> Get(string userName, string loggedUserId);
    }
}

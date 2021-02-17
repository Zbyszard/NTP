using Kopyw.Core.Models;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IUserStatsManager
    {
        Task<UserStats> Get(string userName, string loggedUserId);
    }
}

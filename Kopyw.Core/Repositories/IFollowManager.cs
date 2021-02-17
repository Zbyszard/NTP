using Kopyw.Core.Models;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IFollowManager
    {
        Task<Follow> Add(Follow newFollow);
        Task<Follow> Delete(string authorId, string userId);
    }
}

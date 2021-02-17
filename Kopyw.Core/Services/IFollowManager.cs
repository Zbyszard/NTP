using Kopyw.Core.DTO;
using System.Threading.Tasks;

namespace Kopyw.Core.Services
{
    public interface IFollowDTOManager
    {
        Task<FollowDTO> Add(FollowDTO newFollow);
        Task<FollowDTO> Delete(string authorId, string userId);
    }
}

using AutoMapper;
using Kopyw.Core.DTO;
using Kopyw.Core.Models;
using Kopyw.Core.Repositiories;
using Kopyw.Core.Services;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Services
{
    public class FollowDTOManager : IFollowDTOManager
    {
        private readonly IFollowManager followManager;
        private readonly IMapper mapper;
        public FollowDTOManager(IFollowManager followManager,
            IMapper mapper)
        {
            this.followManager = followManager;
            this.mapper = mapper;
        }
        public async Task<FollowDTO> Add(FollowDTO newFollow)
        {
            if (string.IsNullOrEmpty(newFollow.AuthorId) || string.IsNullOrEmpty(newFollow.ObserverId))
                return null;
            var follow = mapper.Map<Follow>(newFollow);
            var added = await followManager.Add(follow);
            if (added == null)
                return null;
            return mapper.Map<FollowDTO>(added);
        }
        public async Task<FollowDTO> Delete(string authorId, string userId)
        {
            var deleted = await followManager.Delete(authorId, userId);
            if (deleted == null)
                return null;
            return mapper.Map<FollowDTO>(deleted);
        }
    }
}

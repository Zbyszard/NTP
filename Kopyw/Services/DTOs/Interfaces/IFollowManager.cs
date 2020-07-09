using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IFollowDTOManager
    {
        public Task<FollowDTO> Add(FollowDTO newFollow);
        public Task<FollowDTO> Delete(string authorId, string userId);
    }
}

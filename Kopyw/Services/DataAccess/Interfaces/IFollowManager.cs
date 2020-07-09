using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IFollowManager
    {
        public Task<Follow> Add(Follow newFollow);
        public Task<Follow> Delete(string authorId, string userId);
    }
}

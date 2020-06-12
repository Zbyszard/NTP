using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IFollowManager
    {
        public Follow Add(Follow newFollow);
        public Follow Update(Follow follow);
        public Follow Delete(Follow follow);
    }
}

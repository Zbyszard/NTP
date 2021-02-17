using Kopyw.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kopyw.Core.Services
{
    public interface IUserFinder
    {
        Task<ApplicationUser> FindByClaimsPrincipal(ClaimsPrincipal userClaims);
        Task<List<string>> FindIdsByNames(List<string> names);
        Task<List<ApplicationUser>> FindUsersByNames(List<string> names);
        Task<List<string>> SearchUsernames(string str);
    }
}
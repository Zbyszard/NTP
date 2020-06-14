using Kopyw.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kopyw.Services
{
    public class UserFinder
    {
        private readonly UserManager<ApplicationUser> userManager;
        public UserFinder(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ApplicationUser> FindByClaimsPrincipal(ClaimsPrincipal userClaims)
        {
            var claimsIdentity = (ClaimsIdentity)userClaims.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await userManager.FindByIdAsync(userId);
        }
    }
}

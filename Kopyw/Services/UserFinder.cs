using Kopyw.Data;
using Kopyw.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kopyw.Services
{
    public class UserFinder
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        public UserFinder(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            db = dbContext;
            this.userManager = userManager;
        }
        public async Task<ApplicationUser> FindByClaimsPrincipal(ClaimsPrincipal userClaims)
        {
            var claimsIdentity = (ClaimsIdentity)userClaims.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await userManager.FindByIdAsync(userId);
        }
        public async Task<List<string>> FindIdsByNames(List<string> names)
        {
            return await db.Users.Where(u => names.Contains(u.UserName)).Select(u => u.Id).ToListAsync();
        }
        public async Task<List<ApplicationUser>> FindUsersByNames(List<string> names)
        {
            var users = await db.Users.Where(u => names.Contains(u.UserName)).ToListAsync();
            return users;
        }
        public async Task<List<string>> SearchUsernames(string str)
        {
            var names = await (from u in db.Users
                               where u.UserName.ToLower().Contains(str.ToLower())
                               orderby u.UserName
                               select u.UserName).Take(50).ToListAsync();
            return names;
        }
    }
}

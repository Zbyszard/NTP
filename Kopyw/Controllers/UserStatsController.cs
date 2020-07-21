using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Kopyw.DTOs;
using Kopyw.Services;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatsController : ControllerBase
    {
        private readonly UserFinder userFinder;
        private readonly IUserStatsDTOManager statsManager;
        public UserStatsController(UserFinder userFinder,
            IUserStatsDTOManager statsManager)
        {
            this.statsManager = statsManager;
            this.userFinder = userFinder;
        }
        [Route("{userName}")]
        [HttpGet]
        public async Task<ActionResult<UserStatsDTO>> Get(string userName)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var info = await statsManager.Get(userName, user?.Id);
            if (info == null)
                return NotFound();
            return Ok(info);
        }
    }
}

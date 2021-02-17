using System.Threading.Tasks;
using Kopyw.Core.DTO;
using Kopyw.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatsController : ControllerBase
    {
        private readonly IUserFinder userFinder;
        private readonly IUserStatsDTOManager statsManager;
        public UserStatsController(IUserFinder userFinder,
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

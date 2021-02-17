using Kopyw.Core.Models;
using Kopyw.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserFinder userFinder;
        private readonly UserManager<ApplicationUser> userManager;
        public TestController(IUserFinder userFinder, UserManager<ApplicationUser> userManager)
        {
            this.userFinder = userFinder;
            this.userManager = userManager;
        }
        [Route("unauth")]
        [HttpGet]
        public ActionResult Unauth()
        {
            Response.Headers.Add("WWW-Authenticate", "The token expired");
            return Unauthorized();
        }
    }
}

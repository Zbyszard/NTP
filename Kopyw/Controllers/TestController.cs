using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.Models;
using Kopyw.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Primitives;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UserFinder userFinder;
        private readonly UserManager<ApplicationUser> userManager;
        public TestController(UserFinder userFinder, UserManager<ApplicationUser> userManager)
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

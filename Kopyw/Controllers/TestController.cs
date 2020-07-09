using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("unauth")]
        [HttpGet]
        public ActionResult Unauth()
        {
            Response.Headers.Add("WWW-Authenticate", "The token expired");
            return Unauthorized();
        }
    }
}

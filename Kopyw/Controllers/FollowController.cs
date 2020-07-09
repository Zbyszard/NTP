using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowDTOManager followDTOManager;
        private readonly UserFinder userFinder;
        public FollowController(IFollowDTOManager followDTOManager,
            UserFinder userFinder)
        {
            this.followDTOManager = followDTOManager;
            this.userFinder = userFinder;
        }
        [Authorize]
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<FollowDTO>> Add(FollowDTO follow)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            follow.ObserverId = user.Id;
            var added = await followDTOManager.Add(follow);
            if (added == null)
                return NoContent();
            return Ok(added);
        }
        [Authorize]
        [Route("delete/{authorId}")]
        [HttpDelete]
        public async Task<ActionResult<FollowDTO>> Delete(string authorId)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var deleted = await followDTOManager.Delete(authorId, user.Id);
            if (deleted == null)
                return NoContent();
            return Ok(deleted);
        }
    }
}

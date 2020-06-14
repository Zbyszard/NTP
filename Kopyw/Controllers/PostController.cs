using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly UserFinder userFinder;
        private readonly IPostDTOManager postDTOManager;
        public PostController(UserFinder userFinder,
            IPostDTOManager postDTOManager)
        {
            this.userFinder = userFinder;
            this.postDTOManager = postDTOManager;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PostDTO>> Get(long id)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var post = await postDTOManager.Get(id, user?.Id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostDTO>> Add(PostDTO newPost)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            if (user == null)
                return BadRequest();

            newPost.AuthorId = user.Id;
            newPost.AuthorName = user.UserName;
            var added = await postDTOManager.Add(newPost);
            if (added == null) 
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }
    }
}

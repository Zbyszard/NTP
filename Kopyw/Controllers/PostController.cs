using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services;
using Kopyw.Services.DataAccess.Interfaces;
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
        private readonly IPostManager postManager;
        public PostController(UserFinder userFinder,
            IPostDTOManager postDTOManager,
            IPostManager postManager)
        {
            this.userFinder = userFinder;
            this.postDTOManager = postDTOManager;
            this.postManager = postManager;
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
        [HttpGet]
        [Route("user/{userName}/{page}/{count}/{sort}/{sortDir}")]
        public async Task<ActionResult<List<PostDTO>>> GetFromUser(int page, int count, string userName, string sort, string sortDir)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var list = await postDTOManager.GetUserPosts(count, page, userName, user?.Id, sort, sortDir);
            if (list == null || list.Count() == 0)
                return NotFound();
            return Ok(list);
        }
        [HttpGet]
        [Route("user/pages/{userName}/{postsPerPage}")]
        public async Task<ActionResult<int>> GetUserPages(string userName, int postsPerPage)
        {
            if(string.IsNullOrEmpty(userName))
            {
                var user = await userFinder.FindByClaimsPrincipal(User);
                if (user == null)
                    return NotFound();
                userName = user.UserName;
            }
            int pages = postDTOManager.GetUserPages(userName, postsPerPage);
            return Ok(pages);
        }
        [HttpGet]
        [Route("{type}/{page}/{count}/{sort}/{sortDir}")]
        public async Task<ActionResult<List<PostDTO>>> GetRange(string type, int page, int count, string sort, string sortDir)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            List<PostDTO> list;
            switch(type)
            {
                case "new":
                    list = await postDTOManager.GetRange(count, page, user?.Id, "time", sortDir);
                    break;
                case "score":
                    list = await postDTOManager.GetRange(count, page, user?.Id, "score", sortDir);
                    break;
                case "observed":
                case "followed":
                    if (user == null)
                        goto default;
                    list = await postDTOManager.GetFollowedPosts(count, page, user.Id, sort, sortDir);
                    break;
                default:
                    return BadRequest();
            }
            if (list == null || list.Count == 0)
                return NotFound();
            return Ok(list);
        }
        [HttpGet]
        [Route("search/{phrase}/{page}/{count}")]
        public async Task<ActionResult<List<PostDTO>>> Search(string phrase, int page, int count)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var posts = await postDTOManager.Search(phrase, count, page, user?.Id);
            return Ok(posts);
        }
        [HttpGet]
        [Route("search/pages/{phrase}/{postsPerPage}")]
        public ActionResult<int> GetSearchPages(string phrase, int postsPerPage)
        {
            int pages = postDTOManager.GetSearchPages(phrase, postsPerPage);
            return Ok(pages);
        }
        [HttpGet]
        [Route("pages/{postsPerPage}")]
        public ActionResult<int> GetPages(int postsPerPage)
        {
            int pages = postDTOManager.GetPages(postsPerPage);
            return Ok(pages);
        }
        [Authorize]
        [HttpGet]
        [Route("observed/pages/{postsPerPage}")]
        public async Task<ActionResult<int>> GetFollowedPages(int postsPerPage)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            int pages = postDTOManager.GetFollowedPages(user.Id, postsPerPage);
            return Ok(pages);
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
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Edit(PostDTO post)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var result = await postDTOManager.Update(post, user.Id);
            if (result == null)
                return NotFound();
            if (result.Value)
                return Ok();
            else
                return BadRequest();
        }
        [Route("delete")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            bool? result = await postManager.Delete(id, user.Id);
            if (result.Value)
                return Ok();
            if (result == null)
                return NotFound();
            return BadRequest();
        }
        [Route("vote")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostVoteDTO>> AddVote(PostVoteDTO newVote)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            newVote.UserId = user.Id;
            newVote = await postDTOManager.AddVote(newVote);
            if (newVote != null)
                return Ok();
            return NotFound();
        }
        [Route("vote/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<PostVoteDTO>> DeleteVote(long id)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var vote = new PostVoteDTO { PostId = id, UserId = user.Id };
            vote = await postDTOManager.DeleteVote(vote);
            if (vote != null)
                return Ok();
            return NotFound();
        }
    }
}
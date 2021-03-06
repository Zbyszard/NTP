﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Kopyw.Core.DTO;
using Kopyw.Core.Notifications;
using Kopyw.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUserFinder userFinder;
        private readonly IPostDTOManager postManager;
        private readonly IPostNotifier postNotifier;
        public PostController(IUserFinder userFinder,
            IPostDTOManager postDTOManager,
            IPostNotifier postNotifier)
        {
            this.userFinder = userFinder;
            this.postManager = postDTOManager;
            this.postNotifier = postNotifier;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PostDTO>> Get(long id)
        {
            var post = await postManager.Get(id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet]
        [Route("{sort}/{sortOrder}/{count}/{page}")]
        public async Task<ActionResult<List<PostDTO>>> GetPage(int page, int count, string sort, string sortOrder)
        {
            var list = await postManager.GetPage(count, page, sort, sortOrder);
            return Ok(list);
        }
        [HttpGet]
        [Route("pages/{postsPerPage}")]
        public ActionResult<int> GetPages(int postsPerPage)
        {
            int pages = postManager.GetPagesCount(postsPerPage);
            return Ok(pages);
        }
        [HttpGet]
        [Route("user/{userName}/{sort}/{sortOrder}/{count}/{page}")]
        public async Task<ActionResult<List<PostDTO>>> GetFromUser(int page, int count, string userName, string sort, string sortOrder)
        {
            if (string.IsNullOrEmpty(userName))
            {
                var user = await userFinder.FindByClaimsPrincipal(User);
                if (user == null)
                    return NotFound();
                userName = user.UserName;
            }
            var list = await postManager.GetUserPosts(count, page, userName, sort, sortOrder);
            return Ok(list);
        }
        [HttpGet]
        [Route("user/pages/{userName}/{postsPerPage}")]
        public async Task<ActionResult<int>> GetUserPages(int postsPerPage, string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                var user = await userFinder.FindByClaimsPrincipal(User);
                if (user == null)
                    return NotFound();
                userName = user.UserName;
            }
            int pages = postManager.GetUserPagesCount(userName, postsPerPage);
            return Ok(pages);
        }
        [HttpGet]
        [Route("search/{phrase}/{sort}/{sortOrder}/{count}/{page}")]
        public async Task<ActionResult<List<PostDTO>>> Search(string phrase, int page, int count, string sort, string sortOrder)
        {
            var posts = await postManager.Search(phrase, count, page, sort, sortOrder);
            return Ok(posts);
        }
        [HttpGet]
        [Route("search/pages/{phrase}/{postsPerPage}")]
        public ActionResult<int> GetSearchPages(string phrase, int postsPerPage)
        {
            int pages = postManager.GetSearchPagesCount(phrase, postsPerPage);
            return Ok(pages);
        }
        [Authorize]
        [HttpGet]
        [Route("observed/{sort}/{sortOrder}/{count}/{page}")]
        public async Task<ActionResult<List<PostDTO>>> GetFollowed(int page, int count, string sort, string sortOrder)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var posts = await postManager.GetFollowedPosts(count, page, user.Id, sort, sortOrder);
            return posts;
        }
        [Authorize]
        [HttpGet]
        [Route("observed/pages/{postsPerPage}")]
        public async Task<ActionResult<int>> GetFollowedPages(int postsPerPage)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            int pages = postManager.GetFollowedPagesCount(user.Id, postsPerPage);
            return Ok(pages);
        }
        [HttpPost]
        [Route("info")]
        public async Task<ActionResult<List<PostInfoDTO>>> GetPostInfo(List<long> ids)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var info = await postManager.GetInformation(ids, user?.Id);
            return Ok(info);
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
            var added = await postManager.Add(newPost);
            if (added == null) 
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }
        [Authorize]
        [Route("edit")]
        [HttpPut]
        public async Task<ActionResult<PostDTO>> Edit(PostDTO post)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            if (user.Id != post.AuthorId)
                return Forbid();
            var result = await postManager.Update(post);
            if (result == null)
                return NotFound();
            return Ok(post);
        }
        [Route("delete/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<PostDTO>> Delete(long id)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var deleted = await postManager.Delete(id, user.Id);
            if (deleted == null)
                return NotFound();
            return Ok(deleted);
        }
        [Route("vote")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostVoteDTO>> AddVote(PostVoteDTO newVote)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            newVote.UserId = user.Id;
            newVote = await postManager.AddVote(newVote);
            if (newVote == null)
                return NotFound();
            await postNotifier.SendUpdate(newVote.PostId);
            return Ok();
        }
        [Route("vote/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<PostVoteDTO>> DeleteVote(long id)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var vote = new PostVoteDTO { PostId = id, UserId = user.Id };
            vote = await postManager.DeleteVote(vote);
            if (vote == null)
                return NotFound();
            await postNotifier.SendUpdate(id);
            return Ok();
        }
    }
}
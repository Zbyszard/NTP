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
    public class CommentController : ControllerBase
    {
        private readonly IUserFinder userFinder;
        private readonly ICommentDTOManager commentDTOManager;
        private readonly IPostNotifier postNotifier;
        public CommentController(IUserFinder userFinder, 
            ICommentDTOManager commentDTOManager,
            IPostNotifier postNotifier)
        {
            this.userFinder = userFinder;
            this.commentDTOManager = commentDTOManager;
            this.postNotifier = postNotifier;
        }
        [Authorize]
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> Add(CommentDTO newComment)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            if (user == null)
                return Unauthorized();
            if (newComment.PostId == 0)
                return BadRequest();
            newComment.AuthorId = user.Id;
            var added = await commentDTOManager.Add(newComment);
            if (added == null)
                return BadRequest();
            await postNotifier.SendUpdate(newComment.PostId);
            return CreatedAtAction(nameof(GetSingle), new { id = added.Id }, added);
        }
        [Route("single/{id}")]
        [HttpGet]
        public async Task<ActionResult<CommentDTO>> GetSingle(long id)
        {
            var comment = await commentDTOManager.Get(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }
        [Route("{postId}")]
        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> GetComments(long postId)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var comments = await commentDTOManager.GetPage(postId, user?.Id);
            return Ok(comments);
        }
        [Authorize]
        [Route("edit")]
        [HttpPut]
        public async Task<ActionResult<CommentDTO>> Edit(CommentDTO comment)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            if (user.Id != comment.AuthorId)
                return Forbid();
            var updated = await commentDTOManager.Update(comment);
            if (updated == null)
                return NotFound();
            return updated;
        }
        [Authorize]
        [Route("delete/{commentId}")]
        [HttpDelete]
        public async Task<ActionResult<CommentDTO>> Delete(long commentId)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var deleted = await commentDTOManager.Delete(commentId, user.Id);
            if (deleted == null)
                return NotFound();
            return deleted;
        }
        [Authorize]
        [Route("vote")]
        [HttpPost]
        public async Task<ActionResult<CommentVoteDTO>> Vote(CommentVoteDTO vote)
        {
            if (vote.Value == 0)
                return BadRequest();
            var user = await userFinder.FindByClaimsPrincipal(User);
            vote.UserId = user.Id;
            vote = await commentDTOManager.Vote(vote);
            return vote;
        }
        [Authorize]
        [Route("vote/{commentId}")]
        [HttpDelete]
        public async Task<ActionResult<CommentVoteDTO>> DeleteVote(long commentId)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var vote = new CommentVoteDTO { CommentId = commentId, UserId = user.Id };
            var deleted = await commentDTOManager.DeleteVote(vote);
            if (deleted == null)
                return NotFound();
            return deleted;
        }
    }
}

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
    public class CommentController : ControllerBase
    {
        private readonly UserFinder userFinder;
        private readonly ICommentDTOManager commentDTOManager;
        public CommentController(UserFinder userFinder, 
            ICommentDTOManager commentDTOManager)
        {
            this.userFinder = userFinder;
            this.commentDTOManager = commentDTOManager;
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
             return CreatedAtAction(nameof(Get), new { id = added.Id }, added);
        }
        [Route("single/{id}")]
        [HttpGet]
        public async Task<ActionResult<CommentDTO>> Get(long id)
        {
            var comment = await commentDTOManager.Get(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }
        [Route("{postId}")]
        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> GetPage(long postId)
        {
            var user = await userFinder.FindByClaimsPrincipal(User);
            var comments = await commentDTOManager.GetPage(postId, user?.Id);
            return Ok(comments);
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

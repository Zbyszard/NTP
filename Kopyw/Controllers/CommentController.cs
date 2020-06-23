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
        public async Task<ActionResult<List<CommentDTO>>> GetRange(long postId)
        {
            var comments = await commentDTOManager.GetRange(postId);
            if (comments.Count == 0)
                return NotFound();
            return Ok(comments);
        }
    }
}

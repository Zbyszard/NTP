using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.DTOs;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kopyw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostDTOManager postDTOManager;
        public PostController(IPostDTOManager postDTOManager)
        {
            this.postDTOManager = postDTOManager;
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<PostDTO> Get(long id)
        {
            return new PostDTO
            {
                AuthorName = "Andrzej",
                CommentCount = 13,
                Id = 2,
                Score = 44,
                Text = "Pozdrawiam cieplutko",
                Title = "Witam wszystkich",
                UserVote = false
            };
        }
        [HttpPost]
        public ActionResult<PostDTO> Add(PostDTO newPost)
        {
            return CreatedAtAction(nameof(Get), new { id = 313 }, new PostDTO
            {
                AuthorName = "Wacław",
                CommentCount = 22,
                Id = 4,
                Score = 2323,
                Text = "Pozdrawiam zimniutko",
                Title = "Witam prawie wszystkich",
                UserVote = false
            });
        }
    }
}

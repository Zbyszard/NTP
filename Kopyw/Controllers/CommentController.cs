using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kopyw.DTOs;
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
        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult<CommentDTO>> Add(CommentDTO newComment)
        //{

        //}
    }
}

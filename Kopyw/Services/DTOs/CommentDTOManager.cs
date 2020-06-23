using AutoMapper;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services.DataAccess;
using Kopyw.Services.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public class CommentDTOManager : ICommentDTOManager
    {
        private readonly ICommentManager commentManager;
        private readonly IMapper mapper;
        public CommentDTOManager(ICommentManager commentManager,
            IMapper mapper)
        {
            this.commentManager = commentManager;
            this.mapper = mapper;
        }
        public async Task<CommentDTO> Add(CommentDTO newComment)
        {
            var comment = mapper.Map<Comment>(newComment);
            var added = await commentManager.Add(comment);
            if (added != null)
            {
                newComment = mapper.Map<CommentDTO>(comment);
                return newComment;
            }
            return null;
        }
        public async Task<CommentDTO> Get(long id)
        {
            var comment = await commentManager.Get(id);
            if (comment == null)
                return null;
            return mapper.Map<CommentDTO>(comment);
        }

        public async Task<CommentDTO> Delete(long id)
        {
            throw new NotImplementedException();
        }


        public async Task<List<CommentDTO>> GetRange(long postId)
        {
            var comments = await commentManager.GetRange(postId);
            return mapper.Map<List<CommentDTO>>(comments);
        }

        public async Task<CommentDTO> Update(CommentDTO Comment)
        {
            throw new NotImplementedException();
        }
    }
}

using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public class CommentDTOManager : ICommentDTOManager
    {
        public Task<CommentDTO> Add(CommentDTO newComment)
        {
            throw new NotImplementedException();
        }

        public Task<CommentDTO> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommentDTO>> GetRange(int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public Task<CommentDTO> Update(CommentDTO Comment)
        {
            throw new NotImplementedException();
        }
    }
}

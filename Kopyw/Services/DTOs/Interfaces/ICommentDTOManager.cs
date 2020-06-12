using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface ICommentDTOManager
    {
        public CommentDTO Add(CommentDTO newComment);
        //public CommentDTO Get(long id);
        public List<CommentDTO> GetRange(int count, int offset, string sort);
        public CommentDTO Update(CommentDTO Comment);
        public CommentDTO Delete(long id);
    }
}

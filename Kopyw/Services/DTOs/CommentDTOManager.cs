using AutoMapper;
using Kopyw.Data;
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

        public async Task<List<CommentDTO>> GetRange(long postId, string userId)
        {
            var dbcomments = await commentManager.GetRange(postId);
            var comments = mapper.Map<List<CommentDTO>>(dbcomments);
            if(!string.IsNullOrEmpty(userId))
            {
                var userVotes = await commentManager.GetVotes(dbcomments, userId);
                foreach(var vote in userVotes)
                {
                    comments[userVotes.IndexOf(vote)].UserVote = vote.Value;
                }
            }
            return comments;
        }

        public async Task<CommentDTO> Update(CommentDTO Comment)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentVoteDTO> Vote(CommentVoteDTO vote)
        {
            var v = mapper.Map<CommentVote>(vote);
            var added = await commentManager.Vote(v);
            return mapper.Map<CommentVoteDTO>(added);
        }

        public async Task<CommentVoteDTO> DeleteVote(CommentVoteDTO vote)
        {
            var v = mapper.Map<CommentVote>(vote);
            var deleted = await commentManager.DeleteVote(v);
            return mapper.Map<CommentVoteDTO>(deleted);
        }
    }
}

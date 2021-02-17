using AutoMapper;
using Kopyw.Core.DTO;
using Kopyw.Core.Models;
using Kopyw.Core.Repositiories;
using Kopyw.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kopyw.Infrastructure.Services
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

        public async Task<CommentDTO> Delete(long id, string loggedUserId)
        {
            var deleted = await commentManager.Delete(id, loggedUserId);
            return mapper.Map<CommentDTO>(deleted);
        }

        public async Task<List<CommentDTO>> GetPage(long postId, string userId)
        {
            var dbcomments = await commentManager.GetPage(postId);
            var comments = mapper.Map<List<CommentDTO>>(dbcomments);
            if(!string.IsNullOrEmpty(userId))
            {
                var userVotes = commentManager.GetVotes(dbcomments, userId);
                foreach(var vote in userVotes)
                {
                    comments[userVotes.IndexOf(vote)].UserVote = vote.Value;
                }
            }
            return comments;
        }

        public async Task<CommentDTO> Update(CommentDTO comment)
        {
            var dbComment = mapper.Map<Comment>(comment);
            dbComment = await commentManager.Update(dbComment);
            return mapper.Map<CommentDTO>(dbComment);
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

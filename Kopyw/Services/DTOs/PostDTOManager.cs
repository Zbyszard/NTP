using Kopyw.Data;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs
{
    public class PostDTOManager : IPostDTOManager
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostManager postManager;
        public PostDTOManager(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IPostManager postManager)
        {
            db = dbContext;
            this.userManager = userManager;
            this.postManager = postManager;
        }

        public async Task<PostDTO> Add(PostDTO newPost)
        {
            var post = new Post
            {
                Title = newPost.Title,
                Text = newPost.Text,
                AuthorId = newPost.AuthorId
            };
            if (await postManager.Add(post) == null)
                return null;
            newPost.Id = post.Id;
            return newPost;
        }

        public async Task<PostDTO> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<PostDTO> Get(long id, string loggedUserId)
        {
            var post = await (from p in db.Posts
                              join u in db.Users on p.AuthorId equals u.Id
                              where p.Id == id
                              select new PostDTO
                              {
                                  AuthorId = p.AuthorId,
                                  AuthorName = u.UserName,
                                  Id = p.Id,
                                  CommentCount = (from c in db.Comments
                                                  where c.PostId == p.Id
                                                  select c).Count(),
                                  Score = (from pv in db.PostVotes
                                           where pv.PostId == p.Id
                                           select pv).Count(),
                                  Text = p.Text,
                                  Title = p.Title,
                                  UserVote = loggedUserId != null &&
                                            (from uv in db.PostVotes
                                             where uv.UserId == loggedUserId
                                             select uv).Count() == 1
                              }).SingleOrDefaultAsync();
            return post;
        }

        public async Task<List<PostDTO>> GetFollowedPosts(string followedUserName, int count, int offset, string sort, string loggedUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PostDTO>> GetRange(int count, int offset, string sort, string loggedUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PostDTO>> GetUserPosts(string userName, int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public async Task<PostDTO> Update(PostDTO post)
        {
            throw new NotImplementedException();
        }
    }
}
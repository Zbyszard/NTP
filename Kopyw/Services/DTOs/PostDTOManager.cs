using AutoMapper;
using Kopyw.Data;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs
{
    public class PostDTOManager : IPostDTOManager
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostManager postManager;
        private readonly IMapper mapper;
        public PostDTOManager(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IPostManager postManager,
            IMapper mapper)
        {
            db = dbContext;
            this.userManager = userManager;
            this.postManager = postManager;
            this.mapper = mapper;
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
        private IQueryable<PostDTO> PostDTOQuery(string loggedUserId, string sort = null, string sortDir = null)
        {
            var query = (from p in db.Posts
                         join u in db.Users on p.AuthorId equals u.Id
                         select new PostDTO
                         {
                             AuthorId = p.AuthorId,
                             AuthorName = u.UserName,
                             Id = p.Id,
                             Text = p.Text,
                             Title = p.Title,
                             PostTime = p.PostTime,
                             CommentCount = (from c in db.Comments
                                             where c.PostId == p.Id
                                             select c).Count(),
                             Score = (from pv in db.PostVotes
                                      where pv.PostId == p.Id
                                      select pv).Count(),
                             UserVote = loggedUserId != null &&
                                       (from uv in db.PostVotes
                                        where uv.UserId == loggedUserId && uv.PostId == p.Id
                                        select uv).Count() == 1
                         });
            
            return Sort(query, sort, sortDir);
        }
        private IQueryable<PostDTO> Sort(IQueryable<PostDTO> query, string type = null, string dir = null)
        {
            if (type == null || dir == null)
                return query;
            if (type.Equals("score") && dir.Equals("desc"))
                query = query.Select(p => p).OrderByDescending(p => p.Score);
            else if (type.Equals("score"))
                query = query.Select(p => p).OrderBy(p => p.Score);
            else if (type.Equals("time") && dir.Equals("asc"))
                query = query.Select(p => p).OrderBy(p => p.PostTime);
            else
                query = query.Select(p => p).OrderByDescending(p => p.PostTime);
            return query;
        }
        public async Task<PostDTO> Get(long id, string loggedUserId)
        {
            var post = await PostDTOQuery(loggedUserId).Where(p => p.Id == id).SingleOrDefaultAsync();
            return post;
        }
        public int GetFollowedPages(string loggedUserId, int postsPerPage)
        {
            int posts = (from p in db.Posts
                         where p.AuthorId == loggedUserId
                         select p).Count();

            int pages = posts / postsPerPage;
            if (posts % postsPerPage != 0)
                pages++;
            return pages;
        }
        public async Task<List<PostDTO>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortDir)
        {
            var postQuery = (from p in db.Posts
                             join u in db.Users on p.AuthorId equals u.Id
                             join f in db.Follows on u.Id equals f.AuthorId
                             where f.ObserverId == loggedUserId
                             select new PostDTO
                             {
                                 Id = p.Id,
                                 AuthorId = u.Id,
                                 AuthorName = u.UserName,
                                 Text = p.Text,
                                 Title = p.Title,
                                 PostTime = p.PostTime,
                                 CommentCount = (from c in db.Comments
                                                 where c.PostId == p.Id
                                                 select c).Count(),
                                 Score = (from pv in db.PostVotes
                                          where pv.PostId == p.Id
                                          select pv).Count(),
                                 UserVote = loggedUserId != null &&
                                           (from uv in db.PostVotes
                                            where uv.UserId == loggedUserId && uv.PostId == p.Id
                                            select uv).Count() == 1
                             });
            var posts = await Sort(postQuery, sort, sortDir).Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
            
        }
        public int GetPages(int postsPerPage)
        {
            int posts = (from p in db.Posts
                         select p).Count();
            int pages = posts / postsPerPage;
            if (posts % postsPerPage != 0)
                pages++;
            return pages;
        }
        public async Task<List<PostDTO>> GetRange(int count, int page, string loggedUserId, string sort, string sortDir)
        {
            var postQuery = PostDTOQuery(loggedUserId, sort, sortDir);
            var posts = await (from p in postQuery
                               select p).Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }
        public int GetUserPages(string userName, int postsPerPage)
        {
            int posts = (from p in db.Posts
                         join u in db.Users on p.AuthorId equals u.Id
                         where u.UserName == userName
                         select p).Count();

            int pages = posts / postsPerPage;
            if (posts % postsPerPage != 0)
                pages++;
            return pages;
        }
        public async Task<List<PostDTO>> GetUserPosts(int count, int page, string userName, string loggedUserId, string sort, string sortDir)
        {
            var postQuery = PostDTOQuery(loggedUserId, sort, sortDir);
            var posts = await (from p in postQuery
                              where p.AuthorName == userName
                              select p).Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }


        public async Task<bool?> Update(PostDTO post, string loggedUserId)
        {
            var p = await postManager.Get(post.Id);
            if (p == null)
                return null;
            if (p.AuthorId != loggedUserId)
                return false;
            p.Text = post.Text;
            p.Title = post.Title;
            if(await postManager.Update(p) == 1)
                return true;
            else
                return null;
        }

        public async Task<PostVoteDTO> AddVote(PostVoteDTO newVoteDTO)
        {
            var newVote = new PostVote { PostId = newVoteDTO.PostId, UserId = newVoteDTO.UserId };
            if (await postManager.AddVote(newVote) == null)
                return null;
            return newVoteDTO;
        }
        public async Task<PostVoteDTO> DeleteVote(PostVoteDTO voteDTO)
        {
            var vote = new PostVote { PostId = voteDTO.PostId, UserId = voteDTO.UserId };
            if (await postManager.DeleteVote(vote) == null)
                return null;
            return voteDTO;
        }
    }
}
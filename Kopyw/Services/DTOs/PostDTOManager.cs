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
        private readonly IPostManager postManager;
        private readonly IMapper mapper;
        public PostDTOManager(IPostManager postManager,
            IMapper mapper)
        {
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
            newPost.PostTime = post.PostTime;
            return newPost;
        }
        public async Task<PostDTO> Update(PostDTO post)
        {
            var dbPost = mapper.Map<Post>(post);
            dbPost = await postManager.Update(dbPost);
            post = mapper.Map<PostDTO>(dbPost);
            return post;
        }
        public async Task<PostDTO> Get(long id)
        {
            var post = await postManager.Get(id);
            return mapper.Map<PostDTO>(post);
        }
        public async Task<List<PostDTO>> GetPage(int count, int page, string sort, string sortOrder)
        {
            var posts = await postManager.GetPage(count, page, sort, sortOrder);
            var dtos = mapper.Map<List<PostDTO>>(posts);
            return dtos;
        }
        public int GetPagesCount(int postsPerPage)
        {
            return postManager.GetPagesCount(postsPerPage);
        }
        public async Task<List<PostDTO>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder)
        {
            var posts = await postManager.GetUserPosts(count, page, userName, sort, sortOrder);
            var dtos = mapper.Map<List<PostDTO>>(posts);
            return dtos;
        }
        public int GetUserPagesCount(string userName, int postsPerPage)
        {
            return postManager.GetUserPagesCount(userName, postsPerPage);
        }
        public async Task<List<PostDTO>> Search(string phrase, int count, int page, string sort, string sortOrder)
        {
            var posts = await postManager.Search(phrase, count, page, sort, sortOrder);
            var dtos = mapper.Map<List<PostDTO>>(posts);
            return dtos;
        }
        public int GetSearchPagesCount(string phrase, int postsPerPage)
        {
            return postManager.GetSearchPagesCount(phrase, postsPerPage);
        }
        public async Task<List<PostDTO>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder)
        {
            var posts = await postManager.GetFollowedPosts(count, page, loggedUserId, sort, sortOrder);
            var dtos = mapper.Map<List<PostDTO>>(posts);
            return dtos;
        }
        public int GetFollowedPagesCount(string loggedUserId, int postsPerPage)
        {
            return postManager.GetFollowedPagesCount(loggedUserId, postsPerPage);
        }
        public async Task<PostDTO> Delete(long id, string loggeduserId)
        {
            var deleted = await postManager.Delete(id, loggeduserId);
            if (deleted == null)
                return null;
            return mapper.Map<PostDTO>(deleted);
        }
        public async Task<List<PostInfoDTO>> GetInformation(List<long> ids, string loggedUserId)
        {
            var infoList = await postManager.GetInformation(ids, loggedUserId);
            return mapper.Map<List<PostInfoDTO>>(infoList);
        }
        public async Task<PostInfoDTO> GetUpdate(long id)
        {
            var info = await postManager.GetUpdate(id);
            return mapper.Map<PostInfoDTO>(info);
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
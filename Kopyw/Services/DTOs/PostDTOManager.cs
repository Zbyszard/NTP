using Kopyw.Data;
using Kopyw.DTOs;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs
{
    public class PostDTOManager : IPostDTOManager
    {
        private readonly ApplicationDbContext db;
        //private readonly IPostManager postManager;
        public PostDTOManager(
            ApplicationDbContext dbContext/*,
            IPostManager postManager*/)
        {
            db = dbContext;
            //this.postManager = postManager;
        }
        public PostDTO Add(PostDTO newPost)
        {
            return newPost;
        }

        public PostDTO Delete(long id)
        {
            throw new NotImplementedException();
        }

        public PostDTO Get(long id)
        {
            throw new NotImplementedException();
        }

        public List<PostDTO> GetFollowedPosts(string followedUserName, int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public List<PostDTO> GetRange(int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public List<PostDTO> GetUserPosts(string userName, int count, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        public PostDTO Update(PostDTO post)
        {
            throw new NotImplementedException();
        }
    }
}

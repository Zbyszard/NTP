using Kopyw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess.Interfaces
{
    public interface IPostManager
    {
        public Task<Post> Add(Post newPost);
        public Task<Post> Get(long id);
        public Task<List<PostInfo>> GetInformation(List<long> ids, string loggedUserId);
        public Task<List<Post>> GetPage(int count, int page, string sort, string sortOrder);
        public int GetPagesCount(int postsPerPage);
        public Task<List<Post>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder);
        public int GetUserPagesCount(string userName, int postsPerPage);
        public Task<List<Post>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder);
        public int GetFollowedPagesCount(string loggedUserId, int postsPerPage);
        public Task<List<Post>> Search(string phrase, int count, int page, string sort, string sortOrder);
        public int GetSearchPagesCount(string phrase, int postsPerPage);
        public Task<Post> Update(Post post);
        public Task<bool?> Delete(long id, string loggedUserId);
        public Task<PostVote> AddVote(PostVote newVote);
        public Task<PostVote> DeleteVote(PostVote vote);
    }
}

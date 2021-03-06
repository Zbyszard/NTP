﻿using Kopyw.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kopyw.Core.Repositiories
{
    public interface IPostManager
    {
        Task<Post> Add(Post newPost);
        Task<Post> Get(long id);
        Task<List<PostInfo>> GetInformation(List<long> ids, string loggedUserId);
        Task<PostInfo> GetUpdate(long id);
        Task<List<Post>> GetPage(int count, int page, string sort, string sortOrder);
        int GetPagesCount(int postsPerPage);
        Task<List<Post>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder);
        int GetUserPagesCount(string userName, int postsPerPage);
        Task<List<Post>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder);
        int GetFollowedPagesCount(string loggedUserId, int postsPerPage);
        Task<List<Post>> Search(string phrase, int count, int page, string sort, string sortOrder);
        int GetSearchPagesCount(string phrase, int postsPerPage);
        Task<Post> Update(Post post);
        Task<Post> Delete(long id, string loggedUserId);
        Task<PostVote> AddVote(PostVote newVote);
        Task<PostVote> DeleteVote(PostVote vote);
    }
}

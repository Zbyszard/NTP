using IdentityServer4.Extensions;
using Kopyw.Data;
using Kopyw.Models;
using Kopyw.Services.DataAccess.Interfaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Kopyw.Services.DataAccess
{
    public class PostManager : IPostManager
    {
        private readonly ApplicationDbContext db;
        public PostManager(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task<Post> Add(Post newPost)
        {
            db.Add(newPost);
            try
            {
                await db.SaveChangesAsync();
                return newPost;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public async Task<Post> Delete(long id, string loggedUserId)
        {
            var post = await (from p in db.Posts
                              where p.Id == id
                              select p).FirstOrDefaultAsync();
            if (post == null)
                return null;
            if (post.AuthorId != loggedUserId)
                return null;
            db.Entry(post).State = EntityState.Deleted;
            try
            {
                await db.SaveChangesAsync();
                return post;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        private IQueryable<Post> PostQuery()
        {
            var query = (from p in db.Posts
                         select p)
                         .Include(p => p.Author);
            return query;
        }
        private IQueryable<Post> SortedQuery(IQueryable<Post> query, string sort, string sortOrder)
        {
            sort ??= "";
            sortOrder ??= "";
            if (sort == "time")
            {
                if (sortOrder == "desc")
                    return query.OrderByDescending(p => p.PostTime);
                else if (sortOrder == "asc")
                    return query.OrderBy(p => p.PostTime);
            }
            else if (sort == "score")
            {
                if (sortOrder == "desc")
                    return query.OrderByDescending(p => p.Votes.Count);
                else if (sortOrder == "asc")
                    return query.OrderBy(p => p.Votes.Count);
            }
            return query;
        }
        private int CountToPageCount(int count, int postsPerPage)
        {
            int pages = count / postsPerPage;
            if (count % postsPerPage != 0)
                pages++;
            return pages;
        }
        public async Task<List<Post>> GetPage(int count, int page, string sort, string sortOrder)
        {
            var query = SortedQuery(PostQuery(), sort, sortOrder);
            var posts = await query.Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }
        public int GetPagesCount(int postsPerPage)
        {
            var count = PostQuery().Count();
            return CountToPageCount(count, postsPerPage);
        }
        private IQueryable<Post> UserPostsQuery(string userName)
        {
            var query = PostQuery().Where(p => p.Author.UserName == userName);
            return query;
        }
        public async Task<List<Post>> GetUserPosts(int count, int page, string userName, string sort, string sortOrder)
        {
            var query = SortedQuery(UserPostsQuery(userName), sort, sortOrder);
            var posts = await query.Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }
        public int GetUserPagesCount(string userName, int postsPerPage)
        {
            var count = UserPostsQuery(userName).Count();
            return CountToPageCount(count, postsPerPage);
        }
        private IQueryable<Post> FollowedPostsQuery(string loggedUserId)
        {
            var query = (from f in db.Follows
                         join author in db.Users on f.AuthorId equals author.Id
                         join p in db.Posts on author.Id equals p.AuthorId
                         where f.Observer.Id == loggedUserId
                         select p)
                         .Include(p => p.Author);
            return query;
        }
        public async Task<List<Post>> GetFollowedPosts(int count, int page, string loggedUserId, string sort, string sortOrder)
        {
            var query = SortedQuery(FollowedPostsQuery(loggedUserId), sort, sortOrder);
            var posts = await query.Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }
        public int GetFollowedPagesCount(string loggedUserId, int postsPerPage)
        {
            var count = FollowedPostsQuery(loggedUserId).Count();
            return CountToPageCount(count, postsPerPage);
        }
        private IQueryable<Post> SearchQuery(string phrase)
        {
            string lowerPhrase = phrase.ToLower();
            var query = PostQuery().Where(p => p.Author.UserName.ToLower() == lowerPhrase ||
                     p.Title.ToLower().Contains(lowerPhrase) ||
                     p.Text.ToLower().Contains(lowerPhrase));
            return query;
        }
        public async Task<List<Post>> Search(string phrase, int count, int page, string sort, string sortOrder)
        {
            var query = SortedQuery(SearchQuery(phrase), sort, sortOrder);
            var posts = await query.Skip((page - 1) * count).Take(count).ToListAsync();
            return posts;
        }
        public int GetSearchPagesCount(string phrase, int postsPerPage)
        {
            var count = SearchQuery(phrase).Count();
            return CountToPageCount(count, postsPerPage);
        }
        public async Task<Post> Get(long id)
        {
            var post = await PostQuery().Select(p => p).Where(p => p.Id == id).FirstOrDefaultAsync();
            return post;
        }
        public async Task<List<PostInfo>> GetInformation(List<long> ids, string loggedUserId)
        {
            var infos = await (from p in db.Posts
                               where ids.Contains(p.Id)
                               select new PostInfo
                               {
                                   PostId = p.Id,
                                   CommentCount = p.Comments.Count,
                                   Score = p.Votes.Count,
                                   UserVote = (from pv in db.PostVotes
                                               where pv.PostId == p.Id && pv.UserId == loggedUserId
                                               select pv).FirstOrDefault(),
                                   FollowingAuthor = p.Author.FollowedBy.Any(f => f.ObserverId == loggedUserId)
                               }).ToListAsync();
            var sorted = infos.OrderBy(pi => ids.IndexOf(pi.PostId)).ToList();
            return sorted;
        }
        public async Task<PostInfo> GetUpdate(long id)
        {
            var info = await (from p in db.Posts
                              where p.Id == id
                              select new PostInfo
                              {
                                  PostId = p.Id,
                                  CommentCount = p.Comments.Count,
                                  Score = p.Votes.Count
                              }).FirstOrDefaultAsync();
            return info;
        }
        public async Task<Post> Update(Post post)
        {
            var dbPost = await (from p in db.Posts
                                where p.Id == post.Id
                                select p).FirstOrDefaultAsync();
            dbPost.Text = post.Text;
            dbPost.Title = post.Title;
            dbPost.LastEditTime = DateTime.Now;
            try
            {
                db.Entry(dbPost).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                return null;
            }
            return dbPost;
        }

        public async Task<PostVote> AddVote(PostVote newVote)
        {
            var pv = await db.PostVotes.Where(p => p.PostId == newVote.PostId &&
                p.UserId == newVote.UserId).FirstOrDefaultAsync();
            var isSelfVote = await (from p in db.Posts
                                    where p.Id == newVote.PostId && p.AuthorId == newVote.UserId
                                    select p).CountAsync() == 1;
            if(pv == null && newVote.PostId != 0 && !newVote.UserId.IsNullOrEmpty() && !isSelfVote)
            {
                db.PostVotes.Add(newVote);
                await db.SaveChangesAsync();
                return newVote;
            }
            return null;
        }
        public async Task<PostVote> DeleteVote(PostVote vote)
        {
            var pv = await db.PostVotes.Where(p => p.PostId == vote.PostId &&
                p.UserId == vote.UserId).FirstOrDefaultAsync();
            if (pv == null)
                return null;
            db.Entry(pv).State = EntityState.Deleted;
            if (await db.SaveChangesAsync() != 1)
                return null;
            return pv;
        }
    }
}

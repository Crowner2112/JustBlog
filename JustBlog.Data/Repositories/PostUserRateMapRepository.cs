using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Data.Repositories
{
    public class PostUserRateMapRepository : IPostUserRateMapRepository
    {
        private readonly JustBlogDbContext Context;

        public PostUserRateMapRepository(JustBlogDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<PostUserRateMap> GetByUserId(int id)
        {
            return Context.PostUserRateMaps.Where(x => x.UserId == id);
        }

        public bool IsVotedByUserIdAndPostId(int userId, int postId)
        {
            var postUserRates = Context.PostUserRateMaps.FirstOrDefault(x => x.UserId == userId && x.PostId == postId);
            if (postUserRates == null)
                return false;
            else
                return true;
        }

        public bool AddVote(int userId, int postId)
        {
            var item = new PostUserRateMap()
            {
                UserId = userId,
                PostId = postId,
            };
            var post = Context.Posts.Find(postId);
            Context.PostUserRateMaps.Add(item);
            post.RateCount++;
            return true;
        }

        public bool DownVote(int userId, int postId)
        {
            var postUserRates = Context.PostUserRateMaps.FirstOrDefault(x => x.UserId == userId && x.PostId == postId);
            var post = Context.Posts.Find(postId);
            Context.PostUserRateMaps.Remove(postUserRates);
            post.RateCount--;
            return true;
        }
    }
}

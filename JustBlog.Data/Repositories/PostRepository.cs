using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Data.Repositories
{
    public class PostRepository : GenericRepository<Post, int>, IPostRepository
    {
        public PostRepository(JustBlogDbContext context) : base(context)
        {
        }

        public Post GetByUrlAndDate(int year, int month, string urlSlug)
        {
            var prePost = this.DbSet.FirstOrDefault(x => x.UrlSlug == urlSlug.Trim() && x.CreatedOn.Year == year && x.CreatedOn.Month == month);
            if (prePost == null)
                return new Post();
            else
            {
                prePost.Rate = (decimal)prePost.RateCount / prePost.ViewCount;
                prePost.ViewCount++;
                return prePost;
            }
        }

        public bool UpdatePublish(int id)
        {
            var prePost = this.DbSet.FirstOrDefault(x => x.Id == id);
            prePost.Publish = !prePost.Publish;
            return true;
        }

        public IEnumerable<Post> GetPostByTagName(string tagName)
        {
            //to do
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPostByCategoryId(int id)
        {
            var posts = this.DbSet.Where(x => x.CategoryId == id);
            foreach (var post in posts)
            {
                post.Rate = (decimal)post.RateCount / post.ViewCount;
            }
            return posts;
        }

        public IEnumerable<Post> GetMostViewedPost(int size)
        {
            var list = this.DbSet.Where(x => x.Publish && !x.IsDeleted).OrderByDescending(x => x.ViewCount).Take(size);
            foreach (var item in list)
            {
                if (item.RateCount > 0 && item.ViewCount > 0)
                    item.Rate = (decimal)item.RateCount / item.ViewCount;
            }
            return list;
        }

        public IEnumerable<Post> GetHighestPosts(int size)
        {
            var list = this.DbSet.Where(x => x.Publish && !x.IsDeleted);
            var finalList = new List<Post>();
            foreach (var item in list)
            {
                if (item.RateCount > 0 && item.ViewCount > 0)
                    item.Rate = (decimal)item.RateCount / item.ViewCount;
                finalList.Add(item);
            }
            var result = finalList.OrderByDescending(x => x.Rate).Take(size);
            return result;
        }
    }
}
using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.Data.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post, int>
    {
        IEnumerable<Post> GetPostByTagName(string tagName);

        Post GetByUrlAndDate(int year, int month, string urlSlug);

        bool UpdatePublish(int id);

        IEnumerable<Post> GetPostByCategoryId(int id);

        IEnumerable<Post> GetMostViewedPost(int size);

        IEnumerable<Post> GetHighestPosts(int size);
    }
}
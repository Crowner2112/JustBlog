using JustBlog.Data.IRepositories;
using System;
using System.Threading.Tasks;

namespace JustBlog.Data.Infrastructures
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }

        IPostRepository PostRepository { get; }

        ITagRepository TagRepository { get; }

        JustBlogDbContext JustBlogDbContext { get; }

        IPostTagMapRepository PostTagMapRepository { get; }

        IPostUserRateMapRepository PostUserRateMapRepository { get; }

        ICommentRepository CommentRepository { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
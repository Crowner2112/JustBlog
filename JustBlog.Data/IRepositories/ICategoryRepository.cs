using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;

namespace JustBlog.Data.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        string GetNameByUrlSlug(string url);
    }
}
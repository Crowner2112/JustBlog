using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System.Linq;

namespace JustBlog.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(JustBlogDbContext context) : base(context)
        {
        }

        public string GetNameByUrlSlug(string url)
        {
            var category = this.DbSet.Where(x => x.UrlSlug == url).FirstOrDefault();
            return category.Name;
        }
    }
}
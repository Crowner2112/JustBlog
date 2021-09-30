using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Data.Repositories
{
    public class CommentRepository : GenericRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(JustBlogDbContext context) : base(context)
        {
        }

        public IEnumerable<Comment> GetAllCommentByPostId(int id)
        {
            return this.DbSet.Where(x => x.PostId == id);
        }
    }
}
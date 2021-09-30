using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.Data.IRepositories
{
    public interface ICommentRepository : IGenericRepository<Comment, int>
    {
        IEnumerable<Comment> GetAllCommentByPostId(int id);
        void UpdateCommentText(int id, string content);
    }
}
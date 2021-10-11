using JustBlog.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Application.Comments
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(string userId, int postId, string commentText);
        bool DeleteComment(int id);
        bool UpdateComment(int id, string commentText);
        IEnumerable<Comment> GetCommentsByPostId(int postId);
    }
}
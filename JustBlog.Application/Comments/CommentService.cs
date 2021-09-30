using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JustBlog.Application.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public CommentService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> AddCommentAsync(string userId, int postId, string commentText)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            var newComment = new Comment()
            {
                Name = currentUser.FirstName + " " + currentUser.LastName,
                Email = currentUser.Email,
                CommentText = commentText,
                PostId = postId
            };
            await unitOfWork.CommentRepository.AddAsync(newComment);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
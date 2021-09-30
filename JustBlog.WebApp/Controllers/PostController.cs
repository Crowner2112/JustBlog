using JustBlog.Application.Comments;
using JustBlog.Application.Posts;
using JustBlog.Application.Rates;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JustBlog.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IRateService rateService;
        private static int CurrentPostId { get; set; }
        private static int CurrentUserId { get; set; }

        public PostController(IPostService postService, ICommentService commentService, IRateService rateService)
        {
            this.commentService = commentService;
            this.postService = postService;
            this.rateService = rateService;
        }

        public IActionResult Index(int year, int month, string urlSlug)
        {
            var model = postService.GetDetailByUrlAndDate(year, month, urlSlug);
            if (model == null)
                return NoContent();
            else
            {
                CurrentUserId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                CurrentPostId = model.Id;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string commentText)
        {
            
            await commentService.AddCommentAsync(CurrentUserId.ToString(), CurrentPostId, commentText);
            return NoContent();
        }

        [HttpPost]
        public IActionResult ChangeVote()
        {
            rateService.ChangeRate(CurrentUserId, CurrentPostId);
            return NoContent();
        }
    }
}
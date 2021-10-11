using JustBlog.Application.Comments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        public IActionResult GetComments([FromQuery]int postId)
        {
            var model = commentService.GetCommentsByPostId(postId);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromQuery] string userId, [FromQuery] int postId, [FromQuery] string commentText)
        {
            if (!string.IsNullOrEmpty(commentText))
                await commentService.AddCommentAsync(userId, postId, commentText);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteComment([FromQuery] int commentId)
        {
            commentService.DeleteComment(commentId);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateComment([FromQuery] int commentId, [FromQuery] string commentText)
        {
            commentService.UpdateComment(commentId, commentText);
            return NoContent();
        }
    }
}

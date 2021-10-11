using JustBlog.Application.Posts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult GetAllCategory([FromQuery] string keyword = null, [FromQuery] int page = 1)
        {
            Func<Post, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                filter = x => x.Title.ToLower().Contains(keyword);
            }
            int limit = 10;
            int start = (page - 1) * limit;
            var model = this.postService.GetAllPaging(filter, start, limit, true);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetailPost(int id)
        {
            var existCategory = postService.GetById(id);
            if (existCategory != null)
            {
                return Ok(existCategory);
            }
            return NotFound(id);
        }

        [HttpPost]
        public IActionResult CreatePost(CreatePostVm createPostVm)
        {
            var isSuccess = postService.Create(createPostVm);
            if (isSuccess) return Ok(isSuccess);
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeletePost(int id)
        {
            var isSuccess = postService.Delete(id);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(id);
        }

        [HttpPut]
        public IActionResult UpdatePost(CreatePostVm createPostVm)
        {
            var isSuccess = postService.Update(createPostVm);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(createPostVm);
        }

        [HttpGet("mostview/{numberOfPost}")]
        public IActionResult GetMostViewPost(int numberOfPost = 3)
        {
            var model = this.postService.GetMostViewedPost(numberOfPost);
            return Ok(model);
        }

        [HttpGet("highestrate/{numberOfPost}")]
        public IActionResult HighestRate(int numberOfPost = 3)
        {
            var model = this.postService.GetHighestRatePost(numberOfPost);
            return Ok(model);
        }

        [HttpGet("{year}/{month}/{urlSlug}")]
        public IActionResult GetPostByDateAndUrl(int year, int month, string urlSlug)
        {
            var model = postService.GetDetailByUrlAndDate(year, month, urlSlug);
            if (model == null)
                return NoContent();
            else
                return Ok(model);
        }


        [HttpPost("change-publish/{id}")]
        public IActionResult ChangePublish(int id)
        {
            var result = postService.UpdatePublish(id);
            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet("page-number")]
        public IActionResult GetNumberPage()
        {
            var count = this.postService.Count();
            var result = this.postService.NumberPage(count, 10);
            return Ok(result);
        }
    }
}

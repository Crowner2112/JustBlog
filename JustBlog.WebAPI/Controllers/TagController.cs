using JustBlog.Application.Tags;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TagController : ControllerBase
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpGet]
        public IActionResult GetAllTag([FromQuery] string keyword = null, [FromQuery] int page = 1)
        {
            Func<Tag, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                filter = x => x.Name.ToLower().Contains(keyword);
            }
            int limit = 10;
            int start = (page - 1) * limit;
            var model = this.tagService.GetAllPaging(filter, start, limit, true);
            return Ok(model);
        }

        [HttpGet("page-number")]
        public IActionResult GetNumberPage()
        {
            var count = this.tagService.Count();
            var result = this.tagService.NumberPage(count, 10);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetailTag(int id)
        {
            var existCategory = tagService.GetById(id);
            if (existCategory != null)
            {
                return Ok(existCategory);
            }
            return NotFound(id);
        }

        [HttpPost]
        public IActionResult CreateTag(TagVm tagVm)
        {
            var isSuccess = tagService.Create(tagVm);
            if (isSuccess) return Ok(isSuccess);
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteTag([FromQuery] int id)
        {
            var isSuccess = tagService.Delete(id);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(id);
        }

        [HttpPut]
        public IActionResult UpdateTag(TagVm tagVm)
        {
            var isSuccess = tagService.Update(tagVm);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(tagVm);
        }

        [HttpGet("posts-of-tag/{url}/{page}")]
        public IActionResult GetAllPostsByTagUrl([FromQuery] string url, [FromQuery] int page = 1)
        {
            int limit = 5;
            int start = (page - 1) * limit;
            var model = tagService.GetAllPostsByTagUrlPaging(start, limit, url);
            return Ok(model);
        }
    }
}
using JustBlog.Application.Categories;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategory([FromQuery] string keyword=null, [FromQuery] int page = 1)
        {
            Func<Category, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                filter = x => x.Name.ToLower().Contains(keyword);
            }
            int limit = 10;
            int start = (page - 1) * limit;
            var model = this.categoryService.GetAllPaging(filter, start, limit, true);
            return Ok(model);
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var model = this.categoryService.GetAll();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetailCategory(int id)
        {
            var existCategory = categoryService.GetById(id);
            if (existCategory != null)
            {
                return Ok(existCategory);
            }
            return NotFound(id);
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryVm categoryVm)
        {
            var isSuccess = categoryService.Create(categoryVm);
            if (isSuccess) return Ok(isSuccess);
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var isSuccess = categoryService.Delete(id);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(id);
        }

        [HttpPut]
        public IActionResult UpdateCategory(CategoryVm categoryVm)
        {
            var isSuccess = categoryService.Update(categoryVm);
            if (isSuccess) return Ok(isSuccess);
            return NotFound(categoryVm);
        }

        [HttpGet("/post-of-category/{url},{page}")]
        public IActionResult GetPostsByCategory([FromQuery] string url, [FromQuery] int page = 1)
        {
            int limit = 10;
            int start = (page - 1) * limit;
            var model = categoryService.GetAllPagingViewByUrl(url, start, limit);
            return Ok(model);
        }

        [HttpGet("page-number")]
        public IActionResult GetNumberPage()
        {
            var count = this.categoryService.Count();
            var result = this.categoryService.NumberPage(count, 10);
            return Ok(result);
        }
    }
}

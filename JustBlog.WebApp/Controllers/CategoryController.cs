using JustBlog.Application.Categories;
using Microsoft.AspNetCore.Mvc;

namespace JustBlog.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index(string url, int page = 1)
        {
            int limit = 5;
            int start = (page - 1) * limit;
            ViewBag.CategoryName = categoryService.GetCategoryNameByUrlSlug(url);
            ViewBag.limitContent = limit;
            ViewBag.pageCurrent = page;
            int countPost = this.categoryService.Count();
            ViewBag.countPost = countPost;
            ViewBag.numberPage = this.categoryService.NumberPage(countPost, limit);
            var model = this.categoryService.GetAllPagingViewByUrl(url, start, limit);
            return View(model);
        }
    }
}
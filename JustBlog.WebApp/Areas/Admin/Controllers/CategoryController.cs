using JustBlog.Application.Categories;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> logger;
        private readonly ICategoryService categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            this.logger = logger;
            this.categoryService = categoryService;
        }

        public IActionResult Index(string keyword = null, int page = 1)
        {
            if (TempData["PreResult"] != null)
            {
                TempData["Success"] = TempData["PreResult"];
            }
            Func<Category, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                keyword = keyword.ToLower();
                filter = x => x.Name.ToLower().Contains(keyword);
            }
            ViewData["ControllerName"] = "Category";
            int limit = 10;
            int start = (page - 1) * limit;
            ViewBag.pageCurrent = page;
            int countCategory = this.categoryService.Count();
            ViewBag.countCategory = countCategory;
            ViewBag.numberPage = this.categoryService.NumberPage(countCategory, limit);
            var model = this.categoryService.GetAllPaging(filter, start, limit, true);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Tạo mới thất bại";
                return View(categoryVm);
            }
            bool isSuccess;
            try
            {
                isSuccess = this.categoryService.Create(categoryVm);
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            if (isSuccess)
            {
                TempData["PreResult"] = "Tạo mới thành công";
                return RedirectToAction("Index", "Category");
            }
            return View(categoryVm);
        }

        [HttpPost]
        [Authorize(Roles = "BlogOwner")]
        public IActionResult Delete(int id)
        {
            this.categoryService.Delete(id);
            return NoContent();
        }

        public IActionResult Update(int id)
        {
            var category = this.categoryService.GetById(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(CategoryVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Cập nhật thất bại";
                return View(categoryVm);
            }
            bool isSuccess;
            try
            {
                isSuccess = this.categoryService.Update(categoryVm);
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            if (isSuccess)
            {
                TempData["PreResult"] = "Cập nhật thành công";
                return RedirectToAction("Index", "Category");
            }
            return View(categoryVm);
        }
    }
}
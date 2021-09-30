using JustBlog.Application.Tags;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public IActionResult Index(string keyword = null, int page = 1)
        {
            if (TempData["PreResult"] != null)
            {
                TempData["Success"] = TempData["PreResult"];
            }
            Func<Tag, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                keyword = keyword.ToLower();
                filter = x => x.Name.ToLower().Contains(keyword);
            }
            ViewData["ControllerName"] = "Tag";
            int limit = 10;
            int start = (page - 1) * limit;
            ViewBag.pageCurrent = page;
            int countCategory = this.tagService.Count();
            ViewBag.countCategory = countCategory;
            ViewBag.numberPage = this.tagService.NumberPage(countCategory, limit);
            var model = this.tagService.GetAllPaging(filter, start, limit, true);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TagVm tagVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Tạo mới thất bại";
                return View(tagVm);
            }
            tagVm.Count = 0;
            var isSuccess = this.tagService.Create(tagVm);
            if (isSuccess)
            {
                TempData["PreResult"] = "Tạo mới thành công";
                return RedirectToAction("Index", "Tag");
            }
            return View(tagVm);
        }

        [HttpPost]
        [Authorize(Roles = "BlogOwner")]
        public IActionResult Delete(int id)
        {
            this.tagService.Delete(id);
            return NoContent();
        }

        public IActionResult Update(int id)
        {
            var tag = this.tagService.GetById(id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Update(TagVm tagVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Cập nhật thất bại";
                return View(tagVm);
            }
            var isSuccess = this.tagService.Update(tagVm);
            if (isSuccess)
            {
                TempData["PreResult"] = "Cập nhật thành công";
                return RedirectToAction("Index", "Tag");
            }
            return View(tagVm);
        }
    }
}
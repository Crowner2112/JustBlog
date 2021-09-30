using JustBlog.Application.Posts;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        public IActionResult Index(string keyword = null, int page = 1)
        {
            if (TempData["PreResult"] != null)
            {
                TempData["Success"] = TempData["PreResult"];
            }
            Func<Post, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                keyword = keyword.ToLower();
                filter = x => x.Title.ToLower().Contains(keyword);
            }
            ViewData["ControllerName"] = "Post";
            int limit = 10;
            int start = (page - 1) * limit;
            ViewBag.pageCurrent = page;
            int countPost = this.postService.Count();
            ViewBag.countCategory = countPost;
            ViewBag.numberPage = this.postService.NumberPage(countPost, limit);
            var model = this.postService.GetAllPaging(filter, start, limit, true);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePostVm createPostVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Tạo mới thất bại";
                return View(createPostVm);
            }
            bool isSuccess;
            try
            {
                isSuccess = this.postService.Create(createPostVm);
            }
            catch (Exception e)
            {
                isSuccess = false;
            }
            if (isSuccess)
            {
                TempData["PreResult"] = "Tạo mới thành công";
                return RedirectToAction("Index", "Post");
            }
            else
            {
                TempData["PreResult"] = "Tạo thất bại";
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "BlogOwner")]
        public IActionResult Delete(int id)
        {
            this.postService.Delete(id);
            return NoContent();
        }

        public IActionResult Update(int id)
        {
            var post = this.postService.GetById(id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Update(CreatePostVm createPostVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["PreResult"] = "Cập nhật thất bại";
                return View(createPostVm);
            }
            bool isSuccess;
            try
            {
                isSuccess = this.postService.Update(createPostVm);
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            if (isSuccess)
            {
                TempData["PreResult"] = "Cập nhật thành công";
                return RedirectToAction("Index", "Post");
            }
            return View(createPostVm);
        }

        [HttpPost]
        public IActionResult UpdatePublish(int id)
        {
            this.postService.UpdatePublish(id);
            return NoContent();
        }
    }
}
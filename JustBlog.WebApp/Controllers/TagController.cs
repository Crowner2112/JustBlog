using JustBlog.Application.Tags;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.WebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }
        public IActionResult Index(string url, int page = 1)
        {
            int limit = 5;
            int start = (page - 1) * limit;
            ViewBag.limitContent = limit;
            ViewBag.pageCurrent = page;
            ViewBag.tagName = tagService.GetByUrlSlug(url).Name;
            int countPost = this.tagService.CountPostsByTagUrlSlug(url);
            ViewBag.countPost = countPost;
            ViewBag.numberPage = this.tagService.NumberPage(countPost, limit);
            var model = tagService.GetAllPostsByTagUrlPaging(start, limit, url);
            return View(model);
        }
    }
}

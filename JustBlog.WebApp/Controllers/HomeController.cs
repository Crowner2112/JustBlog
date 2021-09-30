using JustBlog.Application.Categories;
using JustBlog.Application.Posts;
using JustBlog.Application.Users;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JustBlog.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly IUserService userService;

        public HomeController(IUserService userService, ICategoryService categoryService, IPostService postService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
            this.postService = postService;
        }

        public IActionResult Index(int page = 1)
        {
            int limit = 5;
            int start = (page - 1) * limit;
            ViewBag.limitContent = limit;
            ViewBag.pageCurrent = page;
            int countPost = this.postService.Count();
            ViewBag.countPost = countPost;
            ViewBag.numberPage = this.postService.NumberPage(countPost, limit);
            var model = this.postService.GetAllPagingView(start, limit);
            return View(model);
        }

        public IActionResult MostView()
        {
            var model = this.postService.GetMostViewedPost(5);
            return View(model);
        }

        public IActionResult HighestRate()
        {
            var model = this.postService.GetHighestRatePost(5);
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(model);
                if (result.Succeeded)
                    return RedirectToAction("index", "Home");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.LoginUserAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await userService.LogoutUserAsync();
            return RedirectToAction("Index");
        }
    }
}
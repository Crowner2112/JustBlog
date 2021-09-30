using Microsoft.AspNetCore.Mvc;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
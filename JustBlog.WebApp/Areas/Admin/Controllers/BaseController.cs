using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "BlogOwner, Contributor")]
    public class BaseController : Controller
    {
    }
}
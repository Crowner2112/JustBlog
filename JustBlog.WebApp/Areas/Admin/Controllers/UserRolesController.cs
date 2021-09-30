using JustBlog.Application.Roles;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "BlogOwner")]
    public class UserRolesController : BaseController
    {
        private readonly IRoleService roleService;

        public UserRolesController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public async Task<IActionResult> Index(string keyword = null, int page = 1)
        {
            if (TempData["PreResult"] != null)
            {
                TempData["Success"] = TempData["PreResult"];
            }
            Func<AppUser, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                keyword = keyword.ToLower();
                filter = x => x.Email.ToLower().Contains(keyword);
            }
            ViewData["ControllerName"] = "UserRoles";
            int limit = 10;
            int start = (page - 1) * limit;
            ViewBag.pageCurrent = page;
            int countCategory = await roleService.CountAsync();
            ViewBag.countCategory = countCategory;
            ViewBag.numberPage = await roleService.NumberPageAsync(countCategory, limit);
            var model = await roleService.GetAllUsersToViewAsync(filter, start, limit);
            return View(model);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await roleService.GetByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = await roleService.GetAllUsersWithRolesAsync(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await roleService.GetByIdAsync(userId);
            if (user == null)
                return View();
            var result = await roleService.RemoveRoleUserAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await roleService.AddRoleAsync(model, user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
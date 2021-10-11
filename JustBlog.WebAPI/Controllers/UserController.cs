using JustBlog.Application.Roles;
using JustBlog.Application.Users;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly IUserService userService;

        public UserController(IRoleService roleService, IUserService userService)
        {
            this.roleService = roleService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(string keyword = null, int page = 1)
        {
            Func<AppUser, bool> filter = null;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                filter = x => x.Email.ToLower().Contains(keyword);
            }
            int limit = 10;
            int start = (page - 1) * limit;
            var model = await roleService.GetAllUsersToViewAsync(filter, start, limit);
            return Ok(model);
        }

        [HttpGet("role/{userId}")]
        public async Task<IActionResult> GetUsersRoleByIdAsync(string userId)
        {
            var user = await roleService.GetByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var model = await roleService.GetAllUsersWithRolesAsync(user);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await roleService.GetByIdAsync(userId);
            if (user == null)
                return BadRequest();
            var result = await roleService.RemoveRoleUserAsync(user);
            if (!result.Succeeded)
                return BadRequest();
            result = await roleService.AddRoleAsync(model, user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await this.userService.GenerateToken(model);
            if (result.Token != null) return Ok(result.Token);
            return BadRequest(result.Message);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await userService.LogoutUserAsync();
            return Ok();
        }
    }
}
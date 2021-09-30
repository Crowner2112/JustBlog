using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Application.Roles
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleService(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<UserRolesViewModel>> GetAllUsersToViewAsync(Func<AppUser, bool> filter, int start, int limit)
        {
            var users = await _userManager.Users.ToListAsync();
            if (filter != null)
            {
                users = users.Where(filter).ToList();
            }
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (AppUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRolesAsync(user);
                userRolesViewModel.Add(thisViewModel);
            }
            var model = userRolesViewModel.OrderBy(x => x.FirstName).Skip(start).Take(limit).ToList();
            return model;
        }

        public async Task<int> CountAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Count;
        }

        public async Task<int> NumberPageAsync(int count, int limit)
        {
            float numberpage = (float)count / limit;
            return (int)Math.Ceiling(numberpage);
        }

        public async Task<List<ManageUserRolesViewModel>> GetAllUsersWithRolesAsync(AppUser user)
        {
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return model;
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> RemoveRoleUserAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> AddRoleAsync(List<ManageUserRolesViewModel> model, AppUser user)
        {
            return await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
        }

        private async Task<List<string>> GetUserRolesAsync(AppUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
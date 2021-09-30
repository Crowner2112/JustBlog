using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustBlog.Application.Roles
{
    public interface IRoleService
    {
        Task<List<UserRolesViewModel>> GetAllUsersToViewAsync(Func<AppUser, bool> filter, int start, int limit);

        Task<AppUser> GetByIdAsync(string userId);

        Task<List<ManageUserRolesViewModel>> GetAllUsersWithRolesAsync(AppUser user);

        Task<IdentityResult> RemoveRoleUserAsync(AppUser user);

        Task<IdentityResult> AddRoleAsync(List<ManageUserRolesViewModel> model, AppUser user);

        Task<int> CountAsync();

        Task<int> NumberPageAsync(int count, int limit);
    }
}
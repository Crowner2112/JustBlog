using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace JustBlog.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole<int>(Enums.Roles.BlogOwner.ToString()));
            await roleManager.CreateAsync(new IdentityRole<int>(Enums.Roles.Contributor.ToString()));
            await roleManager.CreateAsync(new IdentityRole<int>(Enums.Roles.User.ToString()));
        }

        public async static Task SeedBlogOwnerAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser
            {
                UserName = "Crowner",
                Email = "crowner@gmail.com",
                FirstName = "Do",
                LastName = "Duc",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Namdinh@12");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.BlogOwner.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Contributor.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.User.ToString());
                }
            }
        }
    }
}
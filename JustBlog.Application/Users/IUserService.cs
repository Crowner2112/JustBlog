using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JustBlog.Application.Users
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterViewModel model);

        Task<SignInResult> LoginUserAsync(LoginViewModel model);

        Task LogoutUserAsync();
    }
}
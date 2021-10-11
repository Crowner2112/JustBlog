using JustBlog.ViewModels.Accounts;
using JustBlog.ViewModels.IdentityResult;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JustBlog.Application.Users
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterViewModel model);

        Task<SignInResult> LoginUserAsync(LoginViewModel model);

        Task LogoutUserAsync();
        Task<IdentityCustomResult> GenerateToken(LoginViewModel request);
    }
}
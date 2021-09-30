using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JustBlog.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel model)
        {
            MailAddress address = new MailAddress(model.Email);
            string userName = address.User;
            var user = new AppUser
            {
                UserName = userName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Dob = model.Dob,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Data.Enums.Roles.User.ToString());
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }

        public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            var userName = model.Email;
            if (IsValidEmail(model.Email))
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    userName = userExists.UserName;
                }
            }
            return await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, false);
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
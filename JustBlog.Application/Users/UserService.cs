using JustBlog.Models.Entities;
using JustBlog.ViewModels.Accounts;
using JustBlog.ViewModels.IdentityResult;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
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
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Data.Enums.Roles.User.ToString());
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }

        public async Task<IdentityCustomResult> GenerateToken(LoginViewModel request)
        {
            var result = await this.SignInAsync(request);
            if (result.IsSuccessed)
            {
                var user = await this.userManager.FindByEmailAsync(request.Email);
                var roles = await this.userManager.GetRolesAsync(user);
                var role = "User";
                if (roles.Contains("BlogOwner"))
                    role = "BlogOwner";
                else if (roles.Contains("Contributor"))
                    role = "Contributor";
                var claims = new[]
                {
                 new Claim(ClaimTypes.Email,user.Email),
                 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                 new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.Role,role),
                 };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //rsA

                var token = new JwtSecurityToken(
                   issuer: this.configuration["Jwt:Issuer"],
                   audience: this.configuration["Jwt:Audience"],
                   claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                return new SuccessResult(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return new ErrorResult("Login failed");
        }

        public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            var userName = model.Email;
            if (IsValidEmail(model.Email))
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    userName = userExists.UserName;
                }
            }
            return await signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, false);
        }

        public async Task LogoutUserAsync()
        {
            await signInManager.SignOutAsync();
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

        public async Task<IdentityCustomResult> SignInAsync(LoginViewModel request)
        {
            try
            {
                var user = await this.userManager.FindByEmailAsync(request.Email);
                if (user == null) return new ErrorResult("Khong tim thay tai khoan");
                var result = await this.signInManager.PasswordSignInAsync(user, request.Password, true, true);
                if (result.Succeeded) return new SuccessResult();
                return new ErrorResult("Dang nhap khong thanh cong");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
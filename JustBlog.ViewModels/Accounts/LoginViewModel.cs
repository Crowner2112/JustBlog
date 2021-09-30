using System.ComponentModel.DataAnnotations;

namespace JustBlog.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email / UserName")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
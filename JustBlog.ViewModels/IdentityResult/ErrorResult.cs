using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JustBlog.ViewModels.IdentityResult
{
    public class ErrorResult : IdentityCustomResult
    {
        public ErrorResult(string message)
        {
            this.Message = message;
            this.IsSuccessed = false;
        }

        public ErrorResult(IEnumerable<IdentityError> error)
        {
            this.Errors = error;
            this.IsSuccessed = false;
        }

        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
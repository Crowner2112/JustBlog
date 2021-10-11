using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.ViewModels.IdentityResult
{
    public class SuccessResult : IdentityCustomResult
    {
        public SuccessResult()
        {
            this.IsSuccessed = true;
        }

        public SuccessResult(string token)
        {
            this.Token = token;
        }
    }
}

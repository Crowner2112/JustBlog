using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JustBlog.Models.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<PostUserRateMap> PostUserRateMaps { get; set; }
    }
}
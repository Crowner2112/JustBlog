using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Models.Entities
{
    public class PostUserRateMap
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

        public AppUser AppUser { get; set; }
        public Post Post { get; set; }
    }
}

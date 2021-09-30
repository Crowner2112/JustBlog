using JustBlog.Models.BaseEntity;
using System.Collections.Generic;

namespace JustBlog.Models.Entities
{
    public class Category : EntityBase<int>
    {
        public string UrlSlug { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
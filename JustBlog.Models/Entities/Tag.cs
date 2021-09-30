using JustBlog.Models.BaseEntity;
using System.Collections.Generic;

namespace JustBlog.Models.Entities
{
    public class Tag : EntityBase<int>
    {
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }

        public ICollection<PostTagMap> PostTagMaps { get; set; }
    }
}
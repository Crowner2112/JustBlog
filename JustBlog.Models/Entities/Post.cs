using JustBlog.Models.BaseEntity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustBlog.Models.Entities
{
    public class Post : EntityBase<int>
    {
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Title can not be null or empty")]
        [StringLength(20, ErrorMessage = "Title can not be over 20 chars")]
        public string Title { get; set; }

        [DisplayName("Tóm tắt")]
        [Required(ErrorMessage = "Short description can not be null or empty")]
        [StringLength(20, ErrorMessage = "Short description can not be over 20 chars")]
        public string ShortDescription { get; set; }

        [DisplayName("Nội dung")]
        [Required(ErrorMessage = "Post content can not be null or empty")]
        [MinLength(3, ErrorMessage = "Post content can not be less than 3 chars")]
        public string PostContent { get; set; }

        [Required(ErrorMessage = "Url can not be bull")]
        public string UrlSlug { get; set; }

        public int ViewCount { get; set; }
        public int RateCount { get; set; }
        public int TotalRate { get; set; }

        [NotMapped]
        public decimal Rate { get; set; }
        public bool Publish { get; set; } = true;
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<PostUserRateMap> PostUserRateMaps { get; set; }
        public ICollection<PostTagMap> PostTagMaps { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public int? UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
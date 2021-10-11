using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JustBlog.ViewModels.Posts
{
    public class CreatePostVm : BaseEntity
    {
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; }

        [DisplayName("Tóm tắt")]
        [Required(ErrorMessage = "Short description can not be null or empty")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string ShortDescription { get; set; }

        [DisplayName("Nội dung")]
        [Required(ErrorMessage = "Post content can not be null or empty")]
        [StringLength(1000, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string PostContent { get; set; }

        [DisplayName("Đường dẫn")]
        [Required(ErrorMessage = "Url can not be bull")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string UrlSlug { get; set; }

        public bool Publish { get; set; }

        public int? UserId { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage = "Danh mục không được trống")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        public string Tags { get; set; }
    }
}
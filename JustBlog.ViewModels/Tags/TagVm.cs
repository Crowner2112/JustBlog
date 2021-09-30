using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JustBlog.ViewModels.Tags
{
    public class TagVm : BaseEntity
    {
        [DisplayName("Tên Danh Mục")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string Name { get; set; }

        [DisplayName("Link Url")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [Required(ErrorMessage = "{0} không được để trống")]
        public string UrlSlug { get; set; }

        [DisplayName("Mô Tả")]
        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 0)]
        public string Description { get; set; }

        public int Count { get; set; }
    }
}
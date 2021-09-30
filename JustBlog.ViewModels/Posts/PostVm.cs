namespace JustBlog.ViewModels.Posts
{
    public class PostVm : BaseEntity
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string PostContent { get; set; }

        public int ViewCount { get; set; }

        public int RateCount { get; set; }

        public int TotalRate { get; set; }

        public decimal Rate { get; set; }

        public int CategoryId { get; set; }
    }
}
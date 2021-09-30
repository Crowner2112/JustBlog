namespace JustBlog.Application.Rates
{
    public interface IRateService
    {
        bool ChangeRate(int userId, int postId);
    }
}
using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.Data.IRepositories
{
    public interface IPostUserRateMapRepository
    {
        IEnumerable<PostUserRateMap> GetByUserId(int id);

        bool IsVotedByUserIdAndPostId(int userId, int postId);

        bool AddVote(int userId, int postId);

        bool DownVote(int userId, int postId);
    }
}
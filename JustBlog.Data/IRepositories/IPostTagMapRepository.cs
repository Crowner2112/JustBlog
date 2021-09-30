using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.Data.IRepositories
{
    public interface IPostTagMapRepository
    {
        IEnumerable<Tag> GetTagsByPostId(int id);

        IEnumerable<PostTagMap> GetByPostId(int id);
    }
}
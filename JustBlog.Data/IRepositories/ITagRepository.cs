using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using System.Collections.Generic;

namespace JustBlog.Data.IRepositories
{
    public interface ITagRepository : IGenericRepository<Tag, int>
    {
        List<int> AddTagByString(string tagNames);
    }
}
using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Data.Repositories
{
    public class PostTagMapRepository : IPostTagMapRepository
    {
        private readonly JustBlogDbContext Context;

        public PostTagMapRepository(JustBlogDbContext context)
        {
            this.Context = context;
        }

        public IEnumerable<PostTagMap> GetByPostId(int id)
        {
            return Context.PostTagMaps.Where(x => x.PostId == id);
        }

        public IEnumerable<Tag> GetTagsByPostId(int id)
        {
            var postTags = Context.PostTagMaps.Where(x => x.PostId == id);
            var listTags = new List<Tag>();
            foreach (var item in postTags)
            {
                var tag = Context.Tags.Find(item.TagId);
                if (!tag.IsDeleted)
                    listTags.Add(tag);
            }
            return listTags;
        }
    }
}
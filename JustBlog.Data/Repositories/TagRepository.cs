using JustBlog.Data.Helpers;
using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Data.Repositories
{
    public class TagRepository : GenericRepository<Tag, int>, ITagRepository
    {
        public TagRepository(JustBlogDbContext context) : base(context)
        {
        }

        public List<int> AddTagByString(string tagNames)
        {
            var tags = tagNames.Split(';');
            foreach (var tag in tags)
            {
                var trimedTag = tag.Trim();
                var tagExisting = this.DbSet.Where(x => x.Name.ToLower().Equals(trimedTag.ToLower())).FirstOrDefault();
                if (tagExisting == null)
                {
                    var newTag = new Tag()
                    {
                        Name = trimedTag,
                        UrlSlug = UrlHelper.FrientlyUrl(trimedTag)
                    };

                    this.DbSet.Add(newTag);
                }
            }
            this.Context.SaveChanges();
            var tagIds = new List<int>();
            foreach (var tag in tags)
            {
                var trimedTag = tag.Trim();
                var tagExisting = this.DbSet.Where(x => x.Name.ToLower().Equals(trimedTag.ToLower())).FirstOrDefault();
                if (tagExisting != null)
                    tagIds.Add(tagExisting.Id);
            }
            return tagIds;
        }
    }
}
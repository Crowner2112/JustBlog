using JustBlog.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Data
{
    public class DbInitializer
    {
        private readonly JustBlogDbContext _context;

        public DbInitializer(JustBlogDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            //if Category has any data return, Database has seeded.
            if (_context.Categories.Any() && _context.Posts.Any() && _context.Tags.Any())
            {
                return;
            }

            var categories = new List<Category>()
            {
                new Category(){Name="Category 1",},
                new Category(){Name="Category 2"},
                new Category(){Name="Category 3"},
            };

            var posts = new List<Post>()
            {
                new Post(){Title="Post 1", ShortDescription="Short 1", PostContent="This is post 1", UrlSlug="post-1", Category=categories[0]},
                new Post(){Title="Post 2", ShortDescription="Short 2", PostContent="This is post 2", UrlSlug="post-2", Category=categories[0]},
                new Post(){Title="Post 3", ShortDescription="Short 3", PostContent="This is post 3", UrlSlug="post-3", Category=categories[1]},
                new Post(){Title="Post 4", ShortDescription="Short 4", PostContent="This is post 4", UrlSlug="post-4", Category=categories[2]},
            };

            var tags = new List<Tag>()
            {
                new Tag(){Name="Tag 1",UrlSlug="Tag1",Description="This is tag 1"},
                new Tag(){Name="Tag 2"},
                new Tag(){Name="Tag 3", Description="This is tag 3"}
            };

            _context.Posts.AddRange(posts);
            _context.Tags.AddRange(tags);
            _context.SaveChanges();
        }
    }
}
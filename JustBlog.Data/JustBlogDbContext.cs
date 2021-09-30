using JustBlog.Models.BaseEntity;
using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JustBlog.Data
{
    public class JustBlogDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public JustBlogDbContext()
        {
        }

        public JustBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTagMap> PostTagMaps { get; set; }
        public DbSet<PostUserRateMap> PostUserRateMaps { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=JustBlogDB;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.LogTo(Console.WriteLine);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(category =>
            {
                category.Property(c => c.Name).IsRequired();
                category.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Comment>(comment =>
            {
                comment.HasOne(c => c.Post)
                       .WithMany(p => p.Comments)
                       .HasForeignKey(c => c.PostId);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(x => x.Id);
                post.Property(x => x.Title).IsRequired().HasMaxLength(200);
                post.Property(x => x.UrlSlug).IsRequired();
                post.HasOne(p => p.Category)
                    .WithMany(c => c.Posts)
                    .HasForeignKey(p => p.CategoryId);
                post.HasOne(p => p.AppUser)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<Tag>(tag =>
            {
                tag.HasKey(x => x.Id);
                tag.Property(x => x.Name).IsRequired();
                tag.Property(x => x.UrlSlug).IsRequired();
            });

            modelBuilder.Entity<PostTagMap>(postTag =>
            {
                postTag.HasKey(x => new { x.PostId, x.TagId });
                postTag.HasOne(pt => pt.Tag)
                        .WithMany(t => t.PostTagMaps)
                        .HasForeignKey(pt => pt.TagId);

                postTag.HasOne(pt => pt.Post)
                        .WithMany(p => p.PostTagMaps)
                        .HasForeignKey(pt => pt.PostId);
            });

            modelBuilder.Entity<PostUserRateMap>(postTagRate =>
            {
                postTagRate.ToTable("Rates");
                postTagRate.HasKey(x => new { x.PostId, x.UserId });
                postTagRate.HasOne(pu => pu.AppUser)
                        .WithMany(u => u.PostUserRateMaps)
                        .HasForeignKey(pu => pu.UserId);

                postTagRate.HasOne(pt => pt.Post)
                        .WithMany(p => p.PostUserRateMaps)
                        .HasForeignKey(pt => pt.PostId);
            });

            modelBuilder.Entity<AppUser>(user =>
            {
                user.ToTable("AppUsers");
                user.HasKey(x => x.Id);
                user.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
                user.Property(x => x.LastName).HasMaxLength(50).IsRequired();
                user.Property(x => x.Dob).IsRequired();
            });

            modelBuilder.Entity<IdentityRole>(role =>
            {
                role.ToTable("AppRoles");
                role.HasKey(x => x.Id);
            });

            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaims");

            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaims");

            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            BeforSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void BeforSaveChanges()
        {
            var entities = ChangeTracker.Entries();
            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;
                if (entity.Entity is IEntityBase<int> asEntity)
                {
                    if (entity.State == EntityState.Added)
                    {
                        asEntity.CreatedOn = now;
                        asEntity.UpdatedOn = now;
                    }
                    if (entity.State == EntityState.Modified)
                    {
                        asEntity.UpdatedOn = now;
                    }
                }
            }
        }
    }
}
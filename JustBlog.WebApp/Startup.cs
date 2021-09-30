using JustBlog.Application.Categories;
using JustBlog.Application.Comments;
using JustBlog.Application.Posts;
using JustBlog.Application.Rates;
using JustBlog.Application.Roles;
using JustBlog.Application.Tags;
using JustBlog.Application.Users;
using JustBlog.Data;
using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Data.Repositories;
using JustBlog.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace JustBlog.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<JustBlogDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JustBlogConnection"));
                options.LogTo(Console.WriteLine);
            });

            services.AddIdentity<AppUser, IdentityRole<int>>()
                    .AddEntityFrameworkStores<JustBlogDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
            services.AddTransient<DbInitializer>();
            services.AddRazorPages();
            // Addtransition, AddSingleton, AddScopped
            // register repositories
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPostTagMapRepository, PostTagMapRepository>();
            services.AddTransient<IPostUserRateMapRepository, PostUserRateMapRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            // register Services
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IRateService, RateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "category",
                    pattern: "Category/{url}",
                    new { controller = "Category", action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "post",
                    pattern: "Post/{year}/{month}/{urlSlug}",
                    new { controller = "Post", action = "Index" },
                    new { year = @"\d{4}", month = @"\d{2}" }
                    );

                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
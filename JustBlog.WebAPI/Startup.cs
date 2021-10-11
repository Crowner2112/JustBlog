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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.WebAPI
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
            string issuer = Configuration.GetValue<string>("Jwt:Issuer");
            string signingKey = Configuration.GetValue<string>("Jwt:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
            services.AddIdentity<AppUser, IdentityRole<int>>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<JustBlogDbContext>();

            services.AddControllers();
            services.AddDbContext<JustBlogDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("JustBlogConnection"));
                options.LogTo(Console.WriteLine);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JustBlog.WebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                     {
                          new OpenApiSecurityScheme
                            {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                               },
                              Scheme = "oauth2",
                              Name = "Bearer",
                                 In = ParameterLocation.Header,
                     },
                        new List<string>()
                      }
                 });

            });

            services.AddTransient<DbInitializer>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPostTagMapRepository, PostTagMapRepository>();
            services.AddTransient<IPostUserRateMapRepository, PostUserRateMapRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JustBlog.WebAPI v1"));
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
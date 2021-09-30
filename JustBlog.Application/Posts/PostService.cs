using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Application.Posts
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int Count()
        {
            var posts = this.unitOfWork.PostRepository.GetAll(isDeleted: true);
            return posts.Count();
        }

        private List<PostTagMap> GetPostTagMap(string tags)
        {
            var postTagMaps = new List<PostTagMap>();
            if (!string.IsNullOrEmpty(tags))
            {
                var tagIds = this.unitOfWork.TagRepository.AddTagByString(tags);

                foreach (var tagId in tagIds)
                {
                    var postTagMap = new PostTagMap()
                    {
                        TagId = tagId
                    };
                    postTagMaps.Add(postTagMap);
                }
            }
            return postTagMaps;
        }

        public bool Create(CreatePostVm createPostVm)
        {
            try
            {
                var postTagMaps = GetPostTagMap(createPostVm.Tags);
                var post = new Post()
                {
                    Title = createPostVm.Title,
                    CategoryId = createPostVm.CategoryId,
                    ShortDescription = createPostVm.ShortDescription,
                    PostContent = createPostVm.PostContent,
                    UrlSlug = createPostVm.UrlSlug,
                    PostTagMaps = postTagMaps
                };

                this.unitOfWork.PostRepository.Add(post);
                return this.unitOfWork.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool Delete(int id, bool isDeleted = false)
        {
            try
            {
                this.unitOfWork.PostRepository.Delete(isDeleted, id);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't delete this Post");
            }
        }

        public IEnumerable<CreatePostVm> GetAllPaging(Func<Post, bool> filter, int start, int limit, bool isDeleted = false)
        {
            var posts = this.unitOfWork.PostRepository.GetAll(filter, isDeleted);
            var postVms = new List<CreatePostVm>();
            foreach (var post in posts)
            {
                var postVm = new CreatePostVm()
                {
                    Id = post.Id,
                    Title = post.Title,
                    IsDeleted = post.IsDeleted,
                    PostContent = post.PostContent,
                    UrlSlug = post.UrlSlug,
                    Publish = post.Publish,
                    ShortDescription = post.ShortDescription,
                    CreatedOn = post.CreatedOn,
                    UpdatedOn = post.UpdatedOn
                };
                postVms.Add(postVm);
            }
            var result = postVms.OrderBy(x => x.Id).Skip(start).Take(limit);
            return result;
        }

        public IEnumerable<Post> GetAllPagingView(int start, int limit)
        {
            Func<Post, bool> filter = x => x.Publish;
            var posts = this.unitOfWork.PostRepository.GetAll(filter, isDeleted: false);
            foreach (var post in posts)
            {
                if (post.RateCount > 0 && post.ViewCount>0)
                    post.Rate = (decimal)post.RateCount / post.ViewCount;
            }
            var result = posts.OrderBy(x => x.Id).Skip(start).Take(limit);
            return result;
        }

        public CreatePostVm GetById(int id)
        {
            var post = this.unitOfWork.PostRepository.GetById(id);
            var listPostTagMaps = this.unitOfWork.PostTagMapRepository.GetTagsByPostId(id);
            var listTags = listPostTagMaps.Select(x => x.Name);
            string listTagsString = string.Join("; ", listTags);
            var createPostVm = new CreatePostVm()
            {
                Id = post.Id,
                Title = post.Title,
                IsDeleted = post.IsDeleted,
                UrlSlug = post.UrlSlug,
                PostContent = post.PostContent,
                ShortDescription = post.ShortDescription,
                Tags = listTagsString,
                CategoryId = post.CategoryId,
                CreatedOn = post.CreatedOn,
                UpdatedOn = post.UpdatedOn
            };
            return createPostVm;
        }

        public int NumberPage(int count, int limit)
        {
            float numberpage = (float)count / limit;
            return (int)Math.Ceiling(numberpage);
        }

        public bool Update(CreatePostVm createPostVm)
        {
            try
            {
                var listPostTagMaps = this.unitOfWork.PostTagMapRepository.GetByPostId(createPostVm.Id).ToList();
                var postTagMaps = GetPostTagMap(createPostVm.Tags);
                var post = this.unitOfWork.PostRepository.GetById(createPostVm.Id);
                post.Title = createPostVm.Title;
                post.PostContent = createPostVm.PostContent;
                post.IsDeleted = createPostVm.IsDeleted;
                post.UrlSlug = createPostVm.UrlSlug;
                post.ShortDescription = createPostVm.ShortDescription;
                post.CategoryId = createPostVm.CategoryId;
                foreach (var item in postTagMaps)
                {
                    if (listPostTagMaps.FirstOrDefault(x => x.TagId == item.TagId) == null)
                        post.PostTagMaps.Add(item);
                }
                post.UpdatedOn = DateTime.UtcNow;
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Can not update this post");
            }
        }

        public PostVm GetDetailByUrlAndDate(int year, int month, string urlSlug)
        {
            var post = this.unitOfWork.PostRepository.GetByUrlAndDate(year, month, urlSlug);
            if (post == null)
                return new PostVm();
            else
            {
                var postVm = new PostVm()
                {
                    Id = post.Id,
                    Title = post.Title,
                    IsDeleted = post.IsDeleted,
                    ShortDescription = post.ShortDescription,
                    PostContent = post.PostContent,
                    ViewCount = post.ViewCount,
                    RateCount = post.RateCount,
                    Rate = post.Rate,
                    CategoryId = post.CategoryId,
                    CreatedOn = post.CreatedOn,
                    UpdatedOn = post.UpdatedOn
                };
                this.unitOfWork.SaveChanges();
                return postVm;
            }
        }

        public bool UpdatePublish(int id)
        {
            try
            {
                this.unitOfWork.PostRepository.UpdatePublish(id);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't change this post");
            }
            return false;
        }

        public IEnumerable<Post> GetMostViewedPost(int size)
        {
            return this.unitOfWork.PostRepository.GetMostViewedPost(size);
        }

        public IEnumerable<Post> GetHighestRatePost(int size)
        {
            return this.unitOfWork.PostRepository.GetHighestPosts(size);
        }
    }
}
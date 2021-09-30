using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Application.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int Count()
        {
            var categories = this.unitOfWork.CategoryRepository.GetAll(isDeleted: true);
            return categories.Count();
        }

        public bool Create(CategoryVm categoryVm)
        {
            try
            {
                var cate = new Category()
                {
                    Id = categoryVm.Id,
                    Name = categoryVm.Name,
                    IsDeleted = categoryVm.IsDeleted,
                    UrlSlug = categoryVm.UrlSlug,
                    Description = categoryVm.Description,
                    CreatedOn = categoryVm.CreatedOn,
                    UpdatedOn = categoryVm.UpdatedOn
                };

                this.unitOfWork.CategoryRepository.Add(cate);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't Create a new Category");
            }
        }

        public bool Delete(int id, bool isDeleted = false)
        {
            try
            {
                this.unitOfWork.CategoryRepository.Delete(isDeleted, id);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't delete this Category");
            }
        }

        public IEnumerable<CategoryVm> GetAllPaging(Func<Category, bool> filter, int start, int limit, bool isDeleted = false)
        {
            var categories = this.unitOfWork.CategoryRepository.GetAll(filter, isDeleted);
            var categoryVms = new List<CategoryVm>();
            foreach (var category in categories)
            {
                var cateVm = new CategoryVm()
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsDeleted = category.IsDeleted,
                    UrlSlug = category.UrlSlug,
                    Description = category.Description,
                    CreatedOn = category.CreatedOn,
                    UpdatedOn = category.UpdatedOn
                };
                categoryVms.Add(cateVm);
            }
            var result = categoryVms.OrderBy(x => x.Id).Skip(start).Take(limit);
            return result;
        }

        public IEnumerable<CategoryVm> GetAll(bool isDeleted = false)
        {
            var categories = this.unitOfWork.CategoryRepository.GetAll(isDeleted: isDeleted);
            var categoryVms = new List<CategoryVm>();
            foreach (var category in categories)
            {
                var cateVm = new CategoryVm()
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsDeleted = category.IsDeleted,
                    UrlSlug = category.UrlSlug,
                    Description = category.Description,
                    CreatedOn = category.CreatedOn,
                    UpdatedOn = category.UpdatedOn
                };
                categoryVms.Add(cateVm);
            }
            return categoryVms;
        }

        public CategoryVm GetById(int id)
        {
            var category = this.unitOfWork.CategoryRepository.GetById(id);
            var categoryVm = new CategoryVm()
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
                UrlSlug = category.UrlSlug,
                Description = category.Description,
                CreatedOn = category.CreatedOn,
                UpdatedOn = category.UpdatedOn
            };
            return categoryVm;
        }

        public int NumberPage(int count, int limit)
        {
            float numberpage = (float)count / limit;
            return (int)Math.Ceiling(numberpage);
        }

        public bool Update(CategoryVm categoryVm)
        {
            try
            {
                var category = this.unitOfWork.CategoryRepository.GetById(categoryVm.Id);
                category.Name = categoryVm.Name;
                category.IsDeleted = categoryVm.IsDeleted;
                category.UrlSlug = categoryVm.UrlSlug;
                category.Description = categoryVm.Description;
                category.UpdatedOn = DateTime.UtcNow;
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can not update this category");
            }
        }

        public string GetCategoryNameByUrlSlug(string url)
        {
            return this.unitOfWork.CategoryRepository.GetNameByUrlSlug(url);
        }

        public IEnumerable<Post> GetAllPagingViewByUrl(string url, int start, int limit)
        {
            var category = this.unitOfWork.CategoryRepository.Find(x => x.UrlSlug == url, false).FirstOrDefault();
            if (category != null)
            {
                var posts = this.unitOfWork.PostRepository.GetPostByCategoryId(category.Id);
                var result = posts.OrderBy(x => x.Id).Skip(start).Take(limit);
                return result;
            }
            return new List<Post>();
        }
    }
}
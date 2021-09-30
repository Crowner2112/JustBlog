using JustBlog.Models.Entities;
using JustBlog.ViewModels.Categories;
using System;
using System.Collections.Generic;

namespace JustBlog.Application.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryVm> GetAll(bool isDeleted = false);

        IEnumerable<CategoryVm> GetAllPaging(Func<Category, bool> filter, int start, int limit, bool isDeleted = false);

        int Count();

        int NumberPage(int count, int limit);

        bool Create(CategoryVm categoryVm);

        CategoryVm GetById(int id);

        bool Update(CategoryVm categoryVm);

        bool Delete(int id, bool isDeleted = false);

        IEnumerable<Post> GetAllPagingViewByUrl(string url, int start, int limit);

        string GetCategoryNameByUrlSlug(string url);
    }
}
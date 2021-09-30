using JustBlog.Models.Entities;
using JustBlog.ViewModels.Tags;
using System;
using System.Collections.Generic;

namespace JustBlog.Application.Tags
{
    public interface ITagService
    {
        IEnumerable<TagVm> GetAllPaging(Func<Tag, bool> filter, int start, int limit, bool isDeleted = false);

        int Count();

        int NumberPage(int count, int limit);

        IEnumerable<TagVm> GetAll(bool isDeleted = false);

        bool Create(TagVm tagVm);

        TagVm GetById(int id);

        bool Update(TagVm tagVm);

        bool Delete(int id, bool isDeleted = false);
    }
}
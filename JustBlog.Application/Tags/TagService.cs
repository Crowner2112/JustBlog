using JustBlog.Data.Infrastructures;
using JustBlog.Models.Entities;
using JustBlog.ViewModels.Tags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JustBlog.Application.Tags
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int Count()
        {
            var tags = this.unitOfWork.TagRepository.GetAll(isDeleted: true);
            return tags.Count();
        }

        public bool Create(TagVm tagVm)
        {
            try
            {
                var tag = new Tag()
                {
                    Id = tagVm.Id,
                    Name = tagVm.Name,
                    IsDeleted = tagVm.IsDeleted,
                    UrlSlug = tagVm.UrlSlug,
                    Description = tagVm.Description,
                    Count = tagVm.Count,
                    CreatedOn = tagVm.CreatedOn,
                    UpdatedOn = tagVm.UpdatedOn
                };
                this.unitOfWork.TagRepository.Add(tag);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't create a new tag");
            }
        }

        public bool Delete(int id, bool isDeleted = false)
        {
            try
            {
                this.unitOfWork.TagRepository.Delete(isDeleted, id);
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can't delete this tag");
            }
        }

        public IEnumerable<TagVm> GetAll(bool isDeleted = false)
        {
            var tags = this.unitOfWork.TagRepository.GetAll(isDeleted: isDeleted);
            var tagVms = new List<TagVm>();
            foreach (var tag in tags)
            {
                var tagVm = new TagVm()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsDeleted = tag.IsDeleted,
                    UrlSlug = tag.UrlSlug,
                    Description = tag.Description,
                    Count = tag.Count,
                    CreatedOn = tag.CreatedOn,
                    UpdatedOn = tag.UpdatedOn
                };
                tagVms.Add(tagVm);
            }
            return tagVms;
        }

        public IEnumerable<TagVm> GetAllPaging(Func<Tag, bool> filter, int start, int limit, bool isDeleted = false)
        {
            var tags = this.unitOfWork.TagRepository.GetAll(filter, isDeleted);
            var tagVms = new List<TagVm>();
            foreach (var tag in tags)
            {
                var tagVm = new TagVm()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsDeleted = tag.IsDeleted,
                    UrlSlug = tag.UrlSlug,
                    Description = tag.Description,
                    CreatedOn = tag.CreatedOn,
                    UpdatedOn = tag.UpdatedOn
                };
                tagVms.Add(tagVm);
            }
            var result = tagVms.OrderBy(x => x.Id).Skip(start).Take(limit);
            return result;
        }

        public TagVm GetById(int id)
        {
            var tag = this.unitOfWork.TagRepository.GetById(id);
            var tagVm = new TagVm()
            {
                Id = tag.Id,
                Name = tag.Name,
                IsDeleted = tag.IsDeleted,
                UrlSlug = tag.UrlSlug,
                Description = tag.Description,
                Count = tag.Count,
                CreatedOn = tag.CreatedOn,
                UpdatedOn = tag.UpdatedOn
            };
            return tagVm;
        }

        public int NumberPage(int count, int limit)
        {
            float numberpage = (float)count / limit;
            return (int)Math.Ceiling(numberpage);
        }

        public bool Update(TagVm tagVm)
        {
            try
            {
                var tag = this.unitOfWork.TagRepository.GetById(tagVm.Id);
                tag.Name = tagVm.Name;
                tag.IsDeleted = tagVm.IsDeleted;
                tag.UrlSlug = tagVm.UrlSlug;
                tag.Description = tagVm.Description;
                tag.Count = tagVm.Count;
                tag.UpdatedOn = DateTime.UtcNow;
                this.unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Can not update this tag");
            }
        }
    }
}
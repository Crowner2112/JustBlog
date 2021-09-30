using JustBlog.Data;
using JustBlog.Data.Infrastructures;
using JustBlog.Data.IRepositories;
using JustBlog.Data.Repositories;
using JustBlog.Models.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.UnitTests.RepositoryTests
{
   public class CategoryRepositoryTests
    {
        private ICategoryRepository categoryRepository;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            this.unitOfWork = new UnitOfWork(new JustBlogDbContext());
            this.categoryRepository = this.unitOfWork.CategoryRepository;
        }

        [Test]
        public void Create_WhenCalled_ReturnIncrease1RecordInDB()
        {
            var numberOfCategories = this.categoryRepository.Count();

            this.categoryRepository.Add(new Category() { Name = "Category 10" });

            this.unitOfWork.SaveChanges();

            var result = this.categoryRepository.Count();

            Assert.That(result, Is.EqualTo(numberOfCategories + 1));
        }

        [Test]
        public void Delete_WhenCalled_ReturnDecrease1RecordInDB()
        {
            var numberOfCategories = this.categoryRepository.Count();

            this.categoryRepository.DeleteByCondition(x => x.Name == "Category 10");

            this.unitOfWork.SaveChanges();

            var result = this.categoryRepository.Count();

            Assert.That(result, Is.EqualTo(numberOfCategories - 1));
        }
    }
}

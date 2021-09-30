using JustBlog.Models.Entities;
using JustBlog.ViewModels.Posts;
using System;
using System.Collections.Generic;

namespace JustBlog.Application.Posts
{
    public interface IPostService
    {
        bool Create(CreatePostVm createPostVm);

        IEnumerable<CreatePostVm> GetAllPaging(Func<Post, bool> filter, int start, int limit, bool isDeleted = false);

        int Count();

        int NumberPage(int count, int limit);

        CreatePostVm GetById(int id);

        PostVm GetDetailByUrlAndDate(int year, int month, string urlSlug);

        bool UpdatePublish(int id);

        bool Update(CreatePostVm createPostVm);

        bool Delete(int id, bool isDeleted = false);

        IEnumerable<Post> GetAllPagingView(int start, int limit);

        IEnumerable<Post> GetMostViewedPost(int size);

        IEnumerable<Post> GetHighestRatePost(int size);
    }
}
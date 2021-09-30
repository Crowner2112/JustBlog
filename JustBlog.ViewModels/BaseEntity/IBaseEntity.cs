using System;

namespace JustBlog.ViewModels
{
    public interface IBaseEntity
    {
        int Id { get; set; }

        bool IsDeleted { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime UpdatedOn { get; set; }
    }
}
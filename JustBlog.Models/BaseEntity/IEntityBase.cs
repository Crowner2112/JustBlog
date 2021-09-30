using System;

namespace JustBlog.Models.BaseEntity
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }

        bool IsDeleted { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime UpdatedOn { get; set; }
    }
}
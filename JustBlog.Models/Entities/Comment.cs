using JustBlog.Models.BaseEntity;
using System;
using System.ComponentModel.DataAnnotations;

namespace JustBlog.Models.Entities
{
    public class Comment : EntityBase<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string CommentHeader { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
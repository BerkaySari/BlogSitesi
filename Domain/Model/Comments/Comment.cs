using System;
using Core.Entity;

namespace Domain.Model.Comments
{
    public class Comment : Entity<Guid>
    {
        public virtual string CommentText { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Date { get; set; }
        public virtual Guid PostId { get; set; }
    }
}
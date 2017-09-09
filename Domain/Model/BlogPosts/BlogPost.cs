using System;
using Core.Entity;
using Domain.Model.UserInfos;

namespace Domain.Model.BlogPosts
{
    public class BlogPost : Entity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual string Contents { get; set; }
        public virtual string Date { get; set; }
        public virtual string Active { get; set; }
        public virtual UserInfo UserInfo { get; set; }

    }
}
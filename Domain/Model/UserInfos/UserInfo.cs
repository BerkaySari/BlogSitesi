using System;
using System.Collections.Generic;
using Core.Entity;
using Domain.Model.BlogPosts;

namespace Domain.Model.UserInfos
{
    public class UserInfo : Entity<Guid>
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Mail { get; set; }
        public virtual string Date { get; set; }

        private IList<BlogPost> _posts;
        public virtual IList<BlogPost> Posts
        {
            get
            {
                _posts = _posts ?? new List<BlogPost>();
                return _posts;
            }
            set { _posts = value; }
        }
    }
}
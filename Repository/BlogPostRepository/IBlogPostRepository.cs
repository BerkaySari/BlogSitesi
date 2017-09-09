using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Repository;
using Domain.Model.BlogPosts;

namespace Repository.BlogPostRepository
{
    public interface IBlogPostRepository : IRepository<BlogPost, Guid>
    {
        IList<BlogPost> GetActivePosts();
        IList<BlogPost> GetActivePosts(Guid? id);
        IList<BlogPost> GetNotActivePosts();
        IList<BlogPost> GetNotActivePosts(Guid? id);
        IList<BlogPost> GetPosts(string userName);
        IList<BlogPost> GetPosts(Guid? id, string userName);
        IList<BlogPost> GetPosts(Guid? id);
        IList<BlogPost> GetActivePostsAndOrderBy();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Domain.Model.BlogPosts;

namespace Service.BlogPostService
{
    public interface IBlogPostService
    {
        Result<IList<BlogPost>> GetActivePosts();
        Result<IList<BlogPost>> GetActivePosts(Guid? id);
        Result<IList<BlogPost>> GetNotActivePosts();
        Result<IList<BlogPost>> GetNotActivePosts(Guid? id);
        Result<IList<BlogPost>> GetPosts(string userName);
        Result<IList<BlogPost>> GetPosts(Guid? id, string userName);
        Result<IList<BlogPost>> GetPosts(Guid? id);
        Result<IList<BlogPost>> GetActivePostsAndOrderBy();
        Result AddPost(BlogPost model, string userName);
        Result UpdatePost(Guid id, BlogPost model, string userName);
        Result ChangePostActivate(Guid id);
        Result DeletePost(Guid id);
    }
}

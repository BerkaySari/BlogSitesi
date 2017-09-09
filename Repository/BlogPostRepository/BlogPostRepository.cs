using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Repository;
using Domain.Model.BlogPosts;
using Domain.Model.UserInfos;
using NHibernate;

namespace Repository.BlogPostRepository
{
    public class BlogPostRepository : RepositoryBase<BlogPost, Guid>, IBlogPostRepository
    {
        public IList<BlogPost> GetActivePosts()
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Active == "True").OrderBy(u => u.Date).Desc.List();

            return result;
        }

        public IList<BlogPost> GetActivePosts(Guid? id)
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Active == "True").Where(u => u.Id == id).OrderBy(u => u.Date).Desc.List();

            return result;
        }
        public IList<BlogPost> GetNotActivePosts()
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Active == "False").OrderBy(u => u.Date).Desc.List();

            return result;
        }
        public IList<BlogPost> GetNotActivePosts(Guid? id)
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Active == "False").Where(u => u.Id == id).OrderBy(u => u.Date).Desc.List();

            return result;
        }
        public IList<BlogPost> GetPosts(string userName)
        {
            var result = Session.QueryOver<BlogPost>()
                .JoinQueryOver(u => u.UserInfo)
                .Where(x => x.UserName == userName)
                .List();

            return result;
        }
        public IList<BlogPost> GetPosts(Guid? id, string userName)
        {
            var result = Session.QueryOver<BlogPost>()
                .Where(u => u.Id == id)
                .JoinQueryOver(u => u.UserInfo)
                .Where(x => x.UserName == userName)
                .List();

            return result;
        }
        public IList<BlogPost> GetPosts(Guid? id)
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Id == id).List();

            return result;
        }
        public IList<BlogPost> GetActivePostsAndOrderBy()
        {
            var result = Session.QueryOver<BlogPost>().Where(u => u.Active == "True").OrderBy(u => u.Date).Desc.List();

            return result;
        }
    }
}

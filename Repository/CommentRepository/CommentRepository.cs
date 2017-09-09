using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Repository;
using Domain.Model.Comments;
using NHibernate;

namespace Repository.CommentRepository
{
    public class CommentRepository : RepositoryBase<Comment, Guid>, ICommentRepository 
    {
        public IList<Comment> GetComment(Guid? id)
        {
            var result = Session.QueryOver<Comment>().OrderBy(u => u.Date).Desc.Where(u => u.PostId == id).List();

            return result;
        }

    }
}

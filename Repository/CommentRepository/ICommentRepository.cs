using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Repository;
using Domain.Model.Comments;

namespace Repository.CommentRepository
{
    public interface ICommentRepository : IRepository<Comment, Guid>
    {
        IList<Comment> GetComment(Guid? id);
    }
}

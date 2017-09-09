using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Domain.Model.Comments;

namespace Service.CommentService
{
    public interface ICommentService
    {
        Result<IList<Comment>> GetComment(Guid? id);
        Result AddComment(Comment model, string userName);
        Result DeleteComment(Guid id);
    }
}

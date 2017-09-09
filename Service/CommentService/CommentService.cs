using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Entity;
using Domain.Model.Comments;
using Repository.CommentRepository;

namespace Service.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public Result<IList<Comment>> GetComment(Guid? id)
        {
            return Result<IList<Comment>>.AsSuccess(_commentRepository.GetComment(id));
        }
        public Result AddComment(Comment model, string userName)
        {
            var comment = new Comment
            {
                CommentText = model.CommentText,
                PostId = model.PostId,
                UserName = userName == "" ? "Ziyeretçi" : userName,
                Date = DateTime.Now.ToString()
            };

            _commentRepository.Insert(comment);

            return Result.AsSuccess();
        }
        public Result DeleteComment(Guid id)
        {
            var deleteComment = _commentRepository.Get(id);

            _commentRepository.Delete(deleteComment);

            return Result.AsSuccess(deleteComment.Id);
        }
    }
}

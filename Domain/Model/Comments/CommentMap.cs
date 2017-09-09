using Core.Entity;

namespace Domain.Model.Comments
{
    public class CommentMap : EntityMap<Comment>
    {
        public CommentMap()
        {
            Map(x => x.CommentText);
            Map(x => x.UserName);
            Map(x => x.Date);
            Map(x => x.PostId);
        }
    }
}
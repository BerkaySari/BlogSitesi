using Core.Entity;

namespace Domain.Model.BlogPosts
{
    public class BlogPostMap : EntityMap<BlogPost>
    {
        public BlogPostMap()
        {
            Map(x => x.Title);
            Map(x => x.Contents);
            Map(x => x.Date);
            Map(x => x.Active);
            References(p => p.UserInfo, "UserId").ForeignKey("FK_BlogPost_UserId").Not.LazyLoad();
        }
    }
}
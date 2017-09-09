using Core.Entity;

namespace Domain.Model.UserInfos
{
    public class UserInfoMap : EntityMap<UserInfo>
    {
        public UserInfoMap()
        {
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.Mail);
            Map(x => x.Date);


            HasMany(x => x.Posts)
                .KeyColumn("UserId")
                .ForeignKeyConstraintName("FK_BlogPost_UserId")
                .LazyLoad()
                .Cascade
                .AllDeleteOrphan();
        }
    }
}
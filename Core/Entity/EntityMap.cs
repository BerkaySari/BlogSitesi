using System;
using FluentNHibernate.Mapping;

namespace Core.Entity
{
    public class EntityMap<T> : ClassMap<T> where T : Entity<Guid>
    {
        protected EntityMap()
        {
            Id(p => p.Id);
            Map(p => p.CreatedDateTime);
            Map(p => p.ModifiedDateTime);
            Map(p => p.Status);
            Map(p => p.SoftDelete);
            Version(p => p.Version).Generated.Never();
            Cache.NonStrictReadWrite();
        }
    }
}
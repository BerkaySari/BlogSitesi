using System;

namespace Core.Entity
{
    public enum Status
    {
        Passive,
        Active
    }

    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>, IEntityTimeStamp
    {
        public virtual TPrimaryKey Id { get; set; }
        public virtual DateTime CreatedDateTime { get; set; }
        public virtual DateTime? ModifiedDateTime { get; set; }
        public virtual TimeSpan Version { get; set; }
        public virtual Status Status { get; set; }
        public virtual bool SoftDelete { get; set; }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var entity = obj as Entity<TPrimaryKey>;

            if (ReferenceEquals(null, entity))

                return false;
            return entity.Id.Equals(Id);

        }
    }
}
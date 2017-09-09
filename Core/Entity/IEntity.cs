namespace Core.Entity
{

    public interface IEntity
    { }

    public interface IEntity<TPrimaryKey> : IEntity
    {
        TPrimaryKey Id { get; set; }
    }

}
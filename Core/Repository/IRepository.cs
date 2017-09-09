using System.Collections.Generic;
using System.Linq;
using Core.Entity;

namespace Core.Repository
{
    public interface IRepository
    {

    }

    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>
    {
        List<TEntity> GetAll();
        IQueryable<TSubEntity> GetAll<TSubEntity>() where TSubEntity : TEntity;
        TEntity Get(TPrimaryKey key);
        TSubEntity Get<TSubEntity>(TPrimaryKey primaryKey) where TSubEntity : TEntity;
        TPrimaryKey Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TPrimaryKey id);
        void Delete(TEntity entity);
        TEntity Load(TPrimaryKey key);
        TSubEntity Load<TSubEntity>(TPrimaryKey key) where TSubEntity : TEntity;
        void SessionFlush();
        void SaveOrUpdate(TEntity entity);
    }
}
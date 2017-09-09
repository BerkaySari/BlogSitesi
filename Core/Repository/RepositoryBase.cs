using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using NHibernate;
using NHibernate.Linq;

namespace Core.Repository
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        protected ISession Session { get { return UnitOfWork.UnitOfWorkExecuter.Current.Session; } }

        public List<TEntity> GetAll()
        {
            return Session.QueryOver<TEntity>().Cacheable().List<TEntity>().OrderBy(p => p.CreatedDateTime).ToList();
        }

        public IQueryable<TSubEntity> GetAll<TSubEntity>() where TSubEntity : TEntity
        {
            return Session.Query<TSubEntity>().Cacheable();
        }

        public TEntity Get(TPrimaryKey key)
        {
            return Session.Get<TEntity>(key);
        }

        public TSubEntity Get<TSubEntity>(TPrimaryKey primaryKey) where TSubEntity : TEntity
        {
            return Session.Get<TSubEntity>(primaryKey);
        }

        public TPrimaryKey Insert(TEntity entity)
        {
            return (TPrimaryKey)Session.Save(entity);
        }

        public void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        public void Delete(TPrimaryKey id)
        {
            Session.Delete(Session.Load<TEntity>(id));
        }

        public void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        public TEntity Load(TPrimaryKey key)
        {
            return Session.Load<TEntity>(key);
        }

        public TSubEntity Load<TSubEntity>(TPrimaryKey key) where TSubEntity : TEntity
        {
            return Session.Load<TSubEntity>(key);
        }

        public void SessionFlush()
        {
            Session.Flush();
        }

        public void SaveOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }
    }
}
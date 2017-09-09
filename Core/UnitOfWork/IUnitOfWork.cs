using System.Data;

namespace Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        void BeginTransaction(IsolationLevel isolationLevel);
        void Commit();
        void Rollback();
    }
}
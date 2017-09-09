using System;
using System.Data;
using NHibernate;

namespace Core.UnitOfWork
{
    public class UnitOfWorkExecuter : IUnitOfWork
    {
        public static UnitOfWorkExecuter Current
        {
            get { return _current; }
            set { _current = value; }
        }

        [ThreadStatic]
        private static UnitOfWorkExecuter _current;

        public ISession Session { get; private set; }
        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public UnitOfWorkExecuter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            try
            { 
                _transaction.Commit();
            }
            finally
            {
                Session.Close();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Session.Close();
            }
        }
    }
}
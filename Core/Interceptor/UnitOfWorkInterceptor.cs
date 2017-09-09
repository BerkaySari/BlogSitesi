using System;
using System.Data;
using Castle.DynamicProxy;
using Core.Common;
using Core.UnitOfWork;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Core.Interceptor
{
    public class UnitOfWorkInterceptor : InterceptorBase, IInterceptor
    {
        private readonly ISessionFactory _sessionFactory;

        public UnitOfWorkInterceptor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            if (UnitOfWorkExecuter.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }
            try
            {
                UnitOfWorkExecuter.Current = new UnitOfWorkExecuter(_sessionFactory);
                UnitOfWorkExecuter.Current.BeginTransaction(IsolationLevel.ReadCommitted);


                try
                {
                    invocation.Proceed();
                    var result = invocation.ReturnValue as Result;
                    if (result != null && result.IsSuccess)
                    {
                        UnitOfWorkExecuter.Current.Commit();
                    }
                    else
                    {
                        SessionRollback();
                    }
                }
                catch (Exception ex)
                {
                    SetRollBackAndReturn(invocation, new Exception(ex.Message));
                    SetException(ex);
                }
            }
            finally
            {
                UnitOfWorkExecuter.Current = null;
            }
        }
    }
}
using System;
using System.Reflection;
using Castle.DynamicProxy;
using Core.Common;
using Core.UnitOfWork;

namespace Core.Interceptor
{
    public class InterceptorBase
    {
        public void SessionRollback()
        {
            if (UnitOfWork.UnitOfWorkExecuter.Current.Session.IsConnected && UnitOfWork.UnitOfWorkExecuter.Current.Session.IsOpen)
                UnitOfWork.UnitOfWorkExecuter.Current.Rollback();
        }
        public void SetRollBackAndReturn(IInvocation invocation, Exception exception)
        {
            var returnValue = Activator.CreateInstance(invocation.MethodInvocationTarget.ReturnType) as Result;
            if (returnValue == null)
                throw new NotSupportedException("This method not Return Result Type.");
            try
            {
                SessionRollback();
                returnValue.Exception = exception;
                returnValue.Message = exception.Message;
            }
            catch (Exception e)
            {
                returnValue.Exception = e;
                returnValue.Message = "";//Res.Error_GeneralError_NotWait;
                SetException(e);
            }
            finally
            {
                invocation.ReturnValue = returnValue;
            }
        }

        public void SetException(Exception ex)
        {
        }
        public static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            return UnitOfWorkHelper.HasUnitOfWorkAttribute(methodInfo) || UnitOfWorkHelper.IsServiceMethod(methodInfo);
        }
    }
}
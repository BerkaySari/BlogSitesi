using System;

namespace Core.Common
{
    [Serializable]
    public class ServiceCustomResultException : Exception
    {
        public ServiceCustomResultException()
            : base()
        {
        }
        public ServiceCustomResultException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }
}
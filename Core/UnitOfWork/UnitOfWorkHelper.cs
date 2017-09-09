using System;
using System.Reflection;

namespace Core.UnitOfWork
{
    public class UnitOfWorkHelper
    {
        public static bool IsServiceClass(Type type)
        {
            return type.Name.Contains("Service");
        }

        public static bool IsServiceMethod(MethodInfo methodInfo)
        {
            return IsServiceClass(methodInfo.DeclaringType);
        }

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }
    }
}
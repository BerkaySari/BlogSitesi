using System;

namespace Core.UnitOfWork
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    { }
}
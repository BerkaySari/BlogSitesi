using System;
using Core.Entity;
using NHibernate;
using NHibernate.Type;

namespace Core.Interceptor
{
    public class EntityTimeStampInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var entityTimeStamp = entity as IEntityTimeStamp;

            if (entityTimeStamp == null)
                return false;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                string propertyName = propertyNames[i];

                if (propertyName == "CreatedDateTime")
                {
                    state[i] = DateTime.Now;
                    return true;
                }
            }

            return true;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            var entityTimeStamp = entity as IEntityTimeStamp;

            if (entityTimeStamp == null)
                return false;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                var type = types[i];

                string propertyName = propertyNames[i];

                //if (propertyName == "CreatedDateTime")
                //{
                //    currentState[i] = DateTime.Now;
                //    return true;
                //}

                if (propertyName == "ModifiedDateTime")
                {
                    currentState[i] = DateTime.Now;
                    return true;
                }
            }

            return true;
        }


    }
}
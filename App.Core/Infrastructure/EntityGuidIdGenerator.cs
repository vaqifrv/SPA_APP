using System;
using NHibernate.Engine;
using NHibernate.Id;

namespace App.Core.Infrastructure
{
    public class EntityGuidIdGenerator : IIdentifierGenerator
    {
        public object Generate(ISessionImplementor session, object obj)
        {
            var entity = obj as GuidEntityBase;
            if (entity != null && !entity.IsNewEntity)
                return entity.Id;
            return GetGuid();
        }

        protected string GetGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }
    }
}

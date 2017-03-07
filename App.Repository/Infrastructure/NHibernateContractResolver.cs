using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace App.Repository.Infrastructure
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);
            return base.CreateContract(objectType);
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            if (typeof(INHibernateProxy).IsAssignableFrom(objectType))
            {
                return base.GetSerializableMembers(objectType.BaseType);
            }
            else
            {
                return base.GetSerializableMembers(objectType);
            }
        }
    }
}
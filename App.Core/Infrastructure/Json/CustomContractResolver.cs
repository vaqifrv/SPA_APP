using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace App.Core.Infrastructure.Json
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private static readonly List<HaveIncludeAttributeForSet> typesWithAttribute =
            new List<HaveIncludeAttributeForSet>();

        private readonly string setOfProperty = "";
        private bool needSearchIncludePropertyAttributes;
        private Type TypeOfClass;

        public CustomContractResolver()
            : this("")
        {
        }

        public CustomContractResolver(string setOfProperty)
        {
            this.setOfProperty = setOfProperty;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof (INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);
            return base.CreateContract(objectType);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            if (!string.IsNullOrEmpty(setOfProperty))
            {
                var haveTypeAttributes =
                    typesWithAttribute.FirstOrDefault(
                        x => x != null && x.SetOfProperty == setOfProperty && x.TypeOfClass == type);
                if (haveTypeAttributes != null)
                {
                    needSearchIncludePropertyAttributes = haveTypeAttributes.NeedSearchIncludePropertyAttributes;
                }
                else
                {
                    needSearchIncludePropertyAttributes = type
                        .GetProperties()
                        //.Where(p => p.IsDefined(typeof(JsonIncludePropertyAttribute), true))
                        .Where(p => p.GetCustomAttributes(typeof (JsonIncludePropertyAttribute), true)
                            .Where(a => ((JsonIncludePropertyAttribute) a).NameOfSet == setOfProperty)
                            .Count() > 0)
                        .Count() > 0;

                    typesWithAttribute.Add(new HaveIncludeAttributeForSet
                    {
                        NeedSearchIncludePropertyAttributes = needSearchIncludePropertyAttributes,
                        SetOfProperty = setOfProperty,
                        TypeOfClass = type
                    });
                }
            }

            return base.CreateProperties(type, memberSerialization);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = string.IsNullOrEmpty(setOfProperty) || !needSearchIncludePropertyAttributes;
            if (!shouldSerialize)
            {
                var attrs = member.GetCustomAttributes(typeof (JsonIncludePropertyAttribute), true);
                //    shouldSerialize = (attrs.Length == 0);
                foreach (var attr in attrs)
                {
                    if (((JsonIncludePropertyAttribute) attr).NameOfSet == setOfProperty)
                    {
                        shouldSerialize = true;
                        break;
                    }
                }
            }


            property.ShouldSerialize = instance => { return shouldSerialize; };

            return property;
        }
    }

    
}
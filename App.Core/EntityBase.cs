using System;
using App.Core.Infrastructure.Json;

namespace App.Core
{
    //[Serializable]
    public class EntityBase<T> : AbstractEntity
    {
        [JsonIncludeProperty(NameOfSet = "list")]
        public virtual T Id { get; set; }

        public virtual bool IsNewEntity
        {
            get
            {
                var isZero = false;
                var genericType = typeof(T);

                if (genericType == typeof(short))
                    isZero = Int16.Parse(Id.ToString()) == 0;
                //isZero = Id.ToString().Equals("0");
                else if (genericType == typeof(int))
                    isZero = Int32.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(byte))
                    isZero = Int32.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(long))
                    isZero = Int64.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(ushort))
                    isZero = UInt16.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(uint))
                    isZero = UInt32.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(ulong))
                    isZero = UInt64.Parse(Id.ToString()) == 0;
                else if (genericType == typeof(ulong))
                    isZero = UInt64.Parse(Id.ToString()) == 0;

                return isZero;
            }
        }
    }
}

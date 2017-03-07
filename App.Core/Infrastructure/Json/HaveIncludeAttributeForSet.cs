using System;

namespace App.Core.Infrastructure.Json
{
    public class HaveIncludeAttributeForSet
    {
        public string SetOfProperty { get; set; }
        public Type TypeOfClass { get; set; }
        public bool NeedSearchIncludePropertyAttributes { get; set; }
    }
}
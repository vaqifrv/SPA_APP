using System;

namespace App.Core.Infrastructure.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class JsonIncludePropertyAttribute : Attribute
    {
        public string NameOfSet { get; set; }
    }
}
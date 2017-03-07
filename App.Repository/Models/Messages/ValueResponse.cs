using App.Core.Infrastructure.Json;

namespace App.Repository.Models.Messages
{
    public class ValueResponse<T> : ResponseBase
    {
        [JsonIncludeProperty(NameOfSet = "response")]
        public T Value { get; set; }
    }
}
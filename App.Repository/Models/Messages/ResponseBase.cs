using System.Collections.Generic;
using App.Core.Infrastructure.Json;
using App.Core.Validation;

namespace App.Repository.Models.Messages
{
    public class ResponseBase
    {
        [JsonIncludeProperty(NameOfSet = "response")]
        public virtual bool Success { get; set; }
        [JsonIncludeProperty(NameOfSet = "response")]
        public virtual IList<BrokenRule> Errors { get; set; }
    }
}
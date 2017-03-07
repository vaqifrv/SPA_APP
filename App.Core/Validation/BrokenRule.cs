using App.Core.Infrastructure.Json;

namespace App.Core.Validation
{
    public class BrokenRule
    {
        public BrokenRule()
        {
        }

        public BrokenRule(string message, string member)
        {
            Message = message;
            Member = member;
        }

        public BrokenRule(string message)
        {
            Message = message;
            Member = "";
        }
        [JsonIncludeProperty(NameOfSet = "response")]
        public string Member { get; set; }
        [JsonIncludeProperty(NameOfSet = "response")]
        public string Message { get; set; }
    }
}
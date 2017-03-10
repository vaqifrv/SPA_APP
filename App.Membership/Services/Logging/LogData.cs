using App.Membership.Services.Logging.Enums;

namespace App.Membership.Services.Logging
{
    public class LogData
    {

        public int ActionId { get; set; } 
        public LogActionResult LogActionResult { get; set; }
        public Level Level { get; set; }
        public string Description { get; set; }
        public string RightName { get; set; }
        public int EventId { get; set; }
    }


}

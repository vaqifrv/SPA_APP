using System;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Services.Logging
{
    public class LogSearchParams
    {
        public int? LogActionId { get; set; }
        public LogActionResult? ActionResult { get; set; }
        public Level? Level { get; set; }
        public int? RightId { get; set; }
        public string UserName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EventId { get; set; }
        public string ClientIp { get; set; }
        public bool IsAdmin { get; set; } = true;
    }
}

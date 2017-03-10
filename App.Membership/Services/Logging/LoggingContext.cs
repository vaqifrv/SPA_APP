using System;

namespace App.Membership.Services.Logging
{
    public class LoggingContext
    {
        public LoggingContext(string userName, string clientIp, string userAgent)
        {
            UserName = userName;
            ClientIp = clientIp;
            UserAgent = userAgent;
            RequestDate = DateTime.Now;
        }

        public string UserName { get; private set; }
        public string ClientIp { get; private set; }
        public string UserAgent { get; private set; }
        public DateTime RequestDate{ get; private set; } 
    }
}

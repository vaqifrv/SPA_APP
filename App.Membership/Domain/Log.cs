using System;
using App.Membership.Services.Logging.Enums;

namespace App.Membership.Domain
{
    public class Log
    {
        public virtual int Id { get; set; }
        public virtual LogAction Action { get; set; }
        public virtual LogActionResult LogActionResult { get; set; }
        public virtual Level Level { get; set; }
        public virtual string Description { get; set; }
        public virtual Right Right { get; set; }
        public virtual int EventId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string ClientIp { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual string UserName { get; set; }
       
        //public string ActionName { get; set; }
        //public string ControllerName { get; set; }
        

    }
}

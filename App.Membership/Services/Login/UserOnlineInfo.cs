using System;

namespace App.Membership.Services.Login
{
    public class UserOnlineInfo
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public DateTime LastActivityTime { get; set; }
    }
}

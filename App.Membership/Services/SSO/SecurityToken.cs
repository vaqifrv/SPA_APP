using System;

namespace App.Membership.Services.SSO
{
    public class SecurityToken
    {
        public DateTime CreateTime { get; set; }
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}

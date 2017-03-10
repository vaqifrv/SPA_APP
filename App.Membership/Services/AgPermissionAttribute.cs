using System;
using System.Security.Permissions;

namespace App.Membership.Services
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public class AgPermissionAttribute : CodeAccessSecurityAttribute
    {
        public string ParamNames { get; set; }
        public AgPermissionAttribute(SecurityAction action) : base(action) { }

        public override System.Security.IPermission CreatePermission()
        {
            return new AgPermission();
        }
    }
}

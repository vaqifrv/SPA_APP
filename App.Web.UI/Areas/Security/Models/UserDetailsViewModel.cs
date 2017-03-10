using App.Membership.Domain;
using System.Collections.Generic;
using System.Web.Security;

namespace App.Web.UI.Areas.Security.Models
{
    public class UserDetailsViewModel
    {
        public MembershipUser User { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<Right> Rights { get; set; }
        public IList<Log> Logs { get; set; }
    }
}
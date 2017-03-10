using App.Membership.Domain;
using System.Collections.Generic;

namespace App.Web.UI.Areas.Security.Models
{
    public class RolesDictionaryEditViewModel
    {
        public IList<CheckedItem<Right>> CheckedRights { get; set; }
        public Role Role { get; set; }
    }
}
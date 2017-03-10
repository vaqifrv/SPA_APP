using App.Membership.Domain;
using System.Collections.Generic;

namespace App.Web.UI.Areas.Security.Models
{
    public class AdminEditViewModel
    {
        public IList<CheckedItem<Role>> CheckedRoles { get; set; }
        public User User { get; set; }
    }
}
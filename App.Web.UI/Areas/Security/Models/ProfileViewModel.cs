using App.Membership.Domain;
using System.Collections.Generic;

namespace App.Web.UI.Areas.Security.Models
{
    public class ProfileViewModel
    {
        public string username { get; set; }
        public IList<ProfileProperty> propertyList { get; set; }
        public IList<Profile> profileData { get; set; }

    }
}
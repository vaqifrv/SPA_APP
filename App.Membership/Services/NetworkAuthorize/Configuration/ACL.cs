using System.Collections.Generic;
using System.Linq;
using App.Membership.Services.NetworkAuthorize.Abstract;
using App.Membership.Services.NetworkAuthorize.Model;

namespace App.Membership.Services.NetworkAuthorize.Configuration
{
    public class Acl : IAcl
    {
        private NetworkAuthorizeSection _section;

        public Acl()
            : this((NetworkAuthorizeSection)System.Configuration.ConfigurationManager.GetSection("NetworkAuthorize"))
        {

        }

        public Acl(NetworkAuthorizeSection section)
        {
            this._section = section;
        }


        public IList<NetworkAccessRule> Items
        {
            get
            {
                if (_section != null)
                    return ((NetworkAuthorizeSection)_section).Acl.Cast<NetworkAuthorizeElement>().Select<NetworkAuthorizeElement, NetworkAccessRule>(x => new NetworkAccessRule(x)).ToList();
                else
                    return new List<NetworkAccessRule>();
            }
        }

    }
}

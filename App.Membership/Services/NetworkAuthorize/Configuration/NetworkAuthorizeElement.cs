using System;
using System.Configuration;

namespace App.Membership.Services.NetworkAuthorize.Configuration
{
    public class NetworkAuthorizeElement : ConfigurationElement
    {
        [ConfigurationProperty("role", DefaultValue = "", IsRequired = false)]
        public String Role
        {
            get
            {
                return (String)this["role"];
            }
        }

        [ConfigurationProperty("network", DefaultValue = "", IsRequired = false)]
        public String Network
        {
            get
            {
                return (String)this["network"];
            }

        }


        //allow, deny
        [ConfigurationProperty("access", DefaultValue = "allow", IsRequired = true)]
        private string Access
        {
            get
            {
                return (((String)this["access"]).ToLower());
            }

        }

        public bool HasAccess
        {
            get
            {
                return (this.Access == "allow");
            }

        }


    }
}

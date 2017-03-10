using System;
using System.Configuration;

namespace App.Membership.Services.SSO.Configuration
{
    public class SingleSignOnSection : ConfigurationSection
    {
        [ConfigurationProperty("Settings")]
        public SingleSignOnElement Settings
        {
            get
            {
                return (SingleSignOnElement)this["Settings"];
            }
            set
            {

                this["Settings"] = value;
            }
        }

    }

    public class SingleSignOnElement : ConfigurationElement
    {
        [ConfigurationProperty("ServiceUri", DefaultValue = "")]
        public String ServiceUri
        {
            get
            {
                return (String)this["ServiceUri"];
            }
            set
            {

                this["ServiceUri"] = value;
            }
        }

        [ConfigurationProperty("SSOServer", DefaultValue = "")]
        public String SsoServer
        {
            get
            {
                return (String)this["SSOServer"];
            }
            set
            {

                this["SSOServer"] = value;
            }
        }
    }

}

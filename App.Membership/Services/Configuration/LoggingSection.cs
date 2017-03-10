using System;
using System.Configuration;

namespace App.Membership.Services.Configuration
{
    public class LoggingSection : ConfigurationSection
    {
        
        // Create a "platform" element.
        [ConfigurationProperty("platform")]
        public PlatformElement Platform
        {
            get
            {
                return (PlatformElement)this["platform"];
            }
            set
            { this["platform"] = value; }
        }

    }

    // Define the "platform" element
    // with "name" attribute.
    public class PlatformElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "ASPNET", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {

                this["name"] = value;
            }
        }
    }



}
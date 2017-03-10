using System.Configuration;

namespace App.Membership.Services.NetworkAuthorize.Configuration
{
    public class NetworkAuthorizeSection : ConfigurationSection
    {
        [ConfigurationProperty("acl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NetworkAuthorizeCollection), AddItemName="add")]
        public NetworkAuthorizeCollection Acl
        {
            get
            {
                return (NetworkAuthorizeCollection)base["acl"];
            }           
        }

    }



    
}

using System.Configuration;

namespace App.Membership.Services.NetworkAuthorize.Configuration
{
    public class NetworkAuthorizeCollection : ConfigurationElementCollection
    {
        //public void Add(NetworkAuthorizeElement item)
        //{
        //    BaseAdd(item, false);
        //}

        protected override ConfigurationElement CreateNewElement()
        {
            return new NetworkAuthorizeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NetworkAuthorizeElement)element).Network + ((NetworkAuthorizeElement)element).HasAccess.ToString() + ((NetworkAuthorizeElement)element).Role;
        }
    }
}

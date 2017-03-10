using System;
using System.Linq;
using System.Net;
using App.Membership.Services.NetworkAuthorize.Abstract;
using App.Membership.Services.NetworkAuthorize.Model;

namespace App.Membership.Services.NetworkAuthorize
{
    public static class NetworkAuthorizationHelper
    {
        private static IAcl _acl;
        private static IAcl Acl
        {
            get
            {
                if (_acl == null)
                {
                    _acl = ClassFactory.Current.GetService<IAcl>();
                }

                return _acl;

            }
        }

        public static bool HasAccessForIp(string clientIp, string roleName)
        {

            try
            {
                IPAddress clientAddress = IPAddress.Parse(clientIp);
                bool isAllowed = false;
                foreach (NetworkAccessRule element in Acl.Items)
                {

                    if (!String.IsNullOrEmpty(element.Role))
                        if (!element.Role.Split(',').Contains(roleName))
                            continue;

                    bool hasAccess = true;
                    foreach (string item in element.Network.Split(new char[] { ',', ';' }))
                    {
                        string[] address = item.Split(new char[] { '\\', '/' });
                        IPAddress allowedAddress = IPAddress.Parse(address[0]);
                        if (address.Length == 2)
                        {
                            IPAddress mask = IPAddress.Parse(address[1]);
                            if (CheckIsAllowed(clientAddress, allowedAddress, mask))
                                hasAccess = element.HasAccess;
                            else
                                continue;
                        }
                        else
                        {
                            if (allowedAddress.Equals(clientAddress))
                                hasAccess = element.HasAccess;
                            else
                                continue;
                        }

                        if (hasAccess)
                            isAllowed = true;
                        else
                            return false;
                    }
                }

                return isAllowed;
            }
            catch
            {
                // Log the exception, probably something wrong with the configuration
                return false;
            }
        }


        private static bool CheckIsAllowed(IPAddress clientAddress, IPAddress allowedAddress, IPAddress mask)
        {
            try
            {
                IPAddress network1 = GetNetworkAddress(clientAddress, mask);
                IPAddress network2 = GetNetworkAddress(clientAddress, allowedAddress);

                return network1.Equals(network2);
            }
            catch
            {
                return false;
            }
        }

        private static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

    }
}

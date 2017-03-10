using System.Collections.Generic;
using App.Membership.Services.NetworkAuthorize.Model;

namespace App.Membership.Services.NetworkAuthorize.Abstract
{
    public interface IAcl
    {
        IList<NetworkAccessRule> Items {get;}
    }
}

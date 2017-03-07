using System.Security.Principal;

namespace App.Core.Infrastructure.Abstract
{
    public interface ISessionScopeProvider
    {
        object Scope { get; }
        IPrincipal Principal { get; }
        bool UserHasAccess(string right);
    }
}

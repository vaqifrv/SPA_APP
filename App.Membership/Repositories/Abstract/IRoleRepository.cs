using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        ValueResponse<Role> FindByName(string name);
        ListResponse<Role> GetRolesByNames(string[] roles);
        ListResponse<Role> GetAllRoles();
        ListResponse<Role> GetAllRolesForUser(string userName);
    }
}

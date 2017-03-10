using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public interface IRightRepository : IRepository<Right, int>
    {
        ValueResponse<bool> CheckUserHasRight(string username, string rightName);
        ListResponse<Right> GetAllRights();
        ValueResponse<Right> FindByName(string name);
        ListResponse<Right> GetRightsForUser(string username);
    }
}

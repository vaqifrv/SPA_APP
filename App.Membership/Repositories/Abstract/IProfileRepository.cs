using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public interface IProfileRepository : IRepository<Profile, int>
    {
        ListResponse<Profile> GetUserProfile(string username);
        ListResponse<Profile> FindAll();
        ListResponse<Profile> FindAll(string propertyName, object propertyValue, bool ascending);
    }
}

using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public interface IProfilePropertyRepository : IRepository<ProfileProperty, string>
    {
        ListResponse<ProfileProperty> GetAllProperties(); 
    }
}

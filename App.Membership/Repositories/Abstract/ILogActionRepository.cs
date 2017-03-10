using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;

namespace App.Membership.Repositories.Abstract
{
    public interface ILogActionRepository : IRepository<LogAction, int>
    {
        ListResponse<LogAction> GetAllLogActions(); 
    }
}

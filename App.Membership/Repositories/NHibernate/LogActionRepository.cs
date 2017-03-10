using System;
using System.Collections.Generic;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.NHibernate
{
    public class LogActionRepository : RepositoryBase<LogAction, int>, ILogActionRepository
    {
        ListResponse<LogAction> ILogActionRepository.GetAllLogActions()
        {
            try
            {
                var logActionResult = Session.QueryOver<LogAction>().List<LogAction>();
                return new ListResponse<LogAction> { List = logActionResult };
            }
            catch (Exception ex)
            {
                return new ListResponse<LogAction> { Errors = new List<Exception> { ex } };
            }
        }
    }
}

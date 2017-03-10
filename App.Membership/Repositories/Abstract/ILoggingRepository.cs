using System;
using System.Collections.Generic;
using App.Membership.Domain;
using App.Membership.Infrastructure.Messages;
using App.Membership.Services.Logging;

//using Agile.Solutions.Infrastructure.Membership.Repositories.Abstract;

namespace App.Membership.Repositories.Abstract
{
    public interface ILoggingRepository : IRepository<Log, int>
    {

        ListResponse<Log> GetLogs(int startIndex, int pageSize, LogSearchParams searchParams, out int totalItems);

        ListResponse<Log> GetLogsForUser(string userName);

        ListResponse<KeyValuePair<string, int>> GetTopUsedRights(DateTime? beginDate, DateTime? endDate);

    }
}

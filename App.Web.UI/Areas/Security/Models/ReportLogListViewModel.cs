using App.Membership.Domain;
using App.Membership.Services.Logging;
using System.Collections.Generic;

namespace App.Web.UI.Areas.Security.Models
{
    public class ReportLogListViewModel
    {
        public IList<Log> Logs { get; set; }
        public LogSearchParams SearchParams { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
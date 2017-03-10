
using App.Membership.Services.Logging;

namespace App.Web.UI.Areas.Security.Models
{
    public class ReportTopUsedRightsModel
    {
        public LogSearchParams SearchParams { get; set; }
        public string TopUsedRightLabels { get; set; }
        public string TopUsedRightValues { get; set; }
        public int TopUsedRightCount { get; set; }
    }
}
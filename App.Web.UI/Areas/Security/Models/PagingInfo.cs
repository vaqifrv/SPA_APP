using System;

namespace App.Web.UI.Areas.Security.Models
{
    public class PagingInfo
    {
        public int Totalltems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)Totalltems / ItemsPerPage); }
        }
        public int StartIndex
        {
            get
            {
                return (CurrentPage - 1) * ItemsPerPage;
            }
        }

    }
}
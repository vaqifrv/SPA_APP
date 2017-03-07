using System.Collections.Generic;

namespace App.Repository.Models.Messages
{
    public class ListResponse<T> : ResponseBase
    {
        public int TotalItems { get; set; }
        public IList<T> List { get; set; }
    }
}
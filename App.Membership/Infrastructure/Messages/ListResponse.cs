using System.Collections.Generic;

namespace App.Membership.Infrastructure.Messages
{
    public class ListResponse<T> : ResponseBase
    {
        public int TotalItems => List == null || List.Count == 0 ? 0 : List.Count;
        private IList<T> _list;
        public IList<T> List
        {
            get { return _list ?? new List<T>(); }
            set { _list = value; }
        }
    }
}
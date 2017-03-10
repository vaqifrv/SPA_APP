using System;
using System.Collections.Generic;

namespace App.Membership.Infrastructure.Messages
{
    public class ResponseBase
    {
        public virtual bool Success => Errors == null || Errors.Count == 0;

        private IList<Exception> _errors;
        public virtual IList<Exception> Errors
        {
            get { return _errors ?? (_errors = new List<Exception>()); }
            set { _errors = value; }
        }
    }
}
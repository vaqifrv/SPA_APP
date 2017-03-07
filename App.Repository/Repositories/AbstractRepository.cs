using System;
using App.Core.Infrastructure;
using NHibernate;

namespace App.Repository.Repositories
{
    public abstract class AbstractRepository
    {
        private ISession _session;

        protected ISession Session
        {
            get
            {
                return _session ?? (_session = IoC.Current.GetObject<ISession>());
            }
        }

        protected int GetTotalItemsCount<TRoot, TSubType>(IQueryOver<TRoot, TSubType> query)
        {
            return query.Clone().ToRowCountQuery().RowCount();
        }

        protected int GetTotalItemsCount(ISQLQuery query)
        {
            return Int32.Parse(query.UniqueResult().ToString());
        }

        public void SessionInit()
        {
            ISession session = Session;
        }
    }
}
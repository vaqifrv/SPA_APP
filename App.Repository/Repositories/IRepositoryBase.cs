using System.Collections.Generic;
using App.Repository.Models.Messages;
using NHibernate.Criterion;

namespace App.Repository.Repositories
{
    public interface IRepositoryBase<T, in TIdT>
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(TIdT id);
        T FindBy(TIdT id);
        T1 Load<T1>(object id);
        T1 FindBy<T1>(object id);
        object UnproxyWithFlush(object obj);
        object UnproxySafe(object obj);
        ListResponse<T> FindAll();
        ListResponse<T> FindAll(ListRequest<T> request);
        T GetItemByQueryOver(QueryOver<T> queryOver);
        IList<T> GetAllByQueryOver(QueryOver<T> queryOver);
    }
}
using System.Collections.Generic;
using App.Core;
using App.Repository.Models.Messages;
using NHibernate.Criterion;

namespace App.Services.Services
{
    public interface IServiceBase<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        ValueResponse<TEntity> Update(ValueRequest<TEntity> request);
        ValueResponse<TEntity> Update(TEntity entity);
        ListResponse<TEntity> List(ListRequest<TEntity> request);
        ListResponse<TEntity> List();
        ValueResponse<TEntity> GetItem(TKey id);
        ResponseBase Delete(TKey id);
        TEntity GetItemByQueryOver(QueryOver<TEntity> queryOver);
        IList<TEntity> GetAllByQueryOver(QueryOver<TEntity> queryOver);
    }
}
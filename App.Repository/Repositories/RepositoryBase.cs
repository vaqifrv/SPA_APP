using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using App.Repository.Models.Messages;
using App.Core.Infrastructure;

namespace App.Repository.Repositories
{
    public abstract class RepositoryBase<T, TIdT> : AbstractRepository, IRepositoryBase<T, TIdT> where T : class
    {
        public Dictionary<string, AbstractCriterion> NamedCriteries { get; set; }

        public virtual void Delete(TIdT id)
        {
            using (var tx = Session.BeginTransaction())
            {
                try
                {
                    var obj = FindBy(id);
                    if (obj == null)
                    {
                        throw new Exception();
                    }
                    Session.Delete(obj);
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public virtual T1 Load<T1>(object id)
        {
            return Session.Load<T1>(id);
        }

        public virtual T1 FindBy<T1>(object id)
        {
            return Session.Get<T1>(id);
        }

        public virtual T FindBy(TIdT id)
        {
            return Session.Get<T>(id);
        }

        public virtual T Add(T entity)
        {
            using (var tx = Session.BeginTransaction())
            {
                try
                {
                    Session.Save(entity);
                    tx.Commit();
                    return entity;
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public virtual T Update(T entity)
        {
            using (var tx = Session.BeginTransaction())
            {
                try
                {
                    Session.Merge(entity);
                    tx.Commit();
                    return entity;
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public object UnproxyWithFlush(object obj)
        {
            //   if (!NHibernateUtil.IsInitialized(obj))
            NHibernateUtil.Initialize(obj);

            var newObj = Session.GetSessionImplementation().PersistenceContext.Unproxy(obj);
            Session.Evict(newObj);
            Session.Flush();
            return newObj;
        }

        public object UnproxySafe(object obj)
        {
            if (obj != null)
                return
                    Session.GetSessionImplementation().PersistenceContext.Unproxy(obj);
            return null;
        }

        public ListResponse<T> FindAll()
        {
            return FindAll(new ListRequest<T>());
        }

        public ListResponse<T> FindAll(ListRequest<T> request)
        {
            NamedCriteries = new Dictionary<string, AbstractCriterion>();
            var response = new ListResponse<T> { Success = true };
            try
            {
                var query = CriteriaForFindAll(request, Session.QueryOver<T>());

                if (NamedCriteries.Count > 0)
                    foreach (var criteria in NamedCriteries.Values)
                    {
                        query.And(criteria);
                    }

                if (!String.IsNullOrEmpty(request.OrderBy))
                {
                    if (request.OrderBy.Contains(","))
                    {
                        foreach (var order in request.OrderBy.Split(','))
                        {
                            query.UnderlyingCriteria.AddOrder(new Order(order, true));
                        }
                    }
                    else
                    {
                        query.UnderlyingCriteria.AddOrder(new Order(request.OrderBy, request.Ascending));
                    }
                }

                if (request.ItemsPerPage != int.MaxValue && request.ItemsPerPage > 0)
                {
                    response.TotalItems = GetTotalItemsCount(query);
                    query.Skip((request.CurrentPage - 1) * request.ItemsPerPage)
                        .Take(request.ItemsPerPage);
                }

                response.List = query.List<T>();
            }
            catch (Exception exc)
            {
                response.Errors = Logging.LogSysError(exc);
                response.Success = false;
            }

            return response;
        }

        public virtual void DeleteQuick(TIdT id)
        {
            using (var tx = Session.BeginTransaction())
            {
                try
                {
                    Session.Delete(Session.Load(typeof(T), id));
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        protected virtual IQueryOver<T, T> CriteriaForFindAll(ListRequest<T> request, IQueryOver<T, T> query)
        {
            return query;
        }

        public T Refresh(T entity)
        {
            //get latest changes from database (other users/sessions changes, manually, etc..) 
            Session.Refresh(entity);
            return entity;
        }

        public T GetItemByQueryOver(QueryOver<T> queryOver)
        {
            var result = queryOver.GetExecutableQueryOver(Session).List<T>();

            if (result.Count > 0)
            {
                return result[0];
            }

            return null;
        }

        public IList<T> GetAllByQueryOver(QueryOver<T> queryOver)
        {
            IList<T> result = null;

            result = queryOver.GetExecutableQueryOver(Session).List();
            return (IList<T>) result;
        }
    }
}
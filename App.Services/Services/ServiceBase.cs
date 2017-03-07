using System;
using System.Collections.Generic;
using App.Core;
using App.Core.Infrastructure;
using App.Core.Validation;
using App.Repository.Models.Messages;
using App.Repository.Repositories;
using NHibernate.Criterion;

namespace App.Services.Services
{
    public class ServiceBase<TEntity, TKey> : IServiceBase<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        protected IRepositoryBase<TEntity, TKey> Repository;

        public virtual ValueResponse<TEntity> Update(ValueRequest<TEntity> request)
        {
            try
            {
                var response = Validate(request.Value);

                if (!response.Success)
                    return response;

                if (request.Value.IsNewEntity)
                {
                    response.Value = Repository.Add(request.Value);
                }
                else
                {
                    response.Value = Repository.Update(request.Value);
                }

                response.Value = request.Value;

                return response;
            }
            catch (Exception exc)
            {
                var response = new ValueResponse<TEntity> { Success = false };
                response.Errors = Logging.LogSysError(exc);
                return response;
            }
        }

        public virtual ValueResponse<TEntity> Update(TEntity entity)
        {
            var request = new ValueRequest<TEntity>();
            request.Value = entity;

            return Update(request);
        }

        public virtual ListResponse<TEntity> List(ListRequest<TEntity> request)
        {
            var response = new ListResponse<TEntity> { Success = true };
            try
            {
                if (request == null)
                    response = Repository.FindAll();
                else
                    response = Repository.FindAll(request);
            }
            catch (Exception exc)
            {
                response.Errors = Logging.LogSysError(exc);
                response.Success = false;
            }

            return response;
        }

        public virtual ValueResponse<TEntity> GetItem(TKey id)
        {
            var response = new ValueResponse<TEntity> { Success = true };
            try
            {
                var item = Repository.FindBy(id);
                response = new ValueResponse<TEntity>
                {
                    Success = true,
                    Value = item
                };
            }
            catch (Exception exc)
            {
                response.Errors = Logging.LogSysError(exc);
                response.Success = false;
            }

            return response;
        }

        public virtual ResponseBase Delete(TKey id)
        {
            try
            {
                Repository.Delete(id);
                return new ResponseBase { Success = true };
            }
            catch (Exception ex)
            {
                return new ResponseBase { Success = false, Errors = new[] { new BrokenRule(ex.Message) } };
            }
        }

        public ListResponse<TEntity> List()
        {
            return List(null);
        }

        protected virtual void ActionBeforeValidate(GuidEntityBase entity)
        {
        }

        protected virtual ValueResponse<TEntity> Validate(TEntity entity)
        {
            var response = new ValueResponse<TEntity> { Success = false };

            try
            {
                response.Errors = CustomModelValidator.Validate(entity);
                response.Success = (response.Errors.Count == 0);
            }
            catch (Exception exc)
            {
                response.Errors = Logging.LogSysError(exc);
            }

            return response;
        }

        public TEntity GetItemByQueryOver(QueryOver<TEntity> queryOver)
        {
            return Repository.GetItemByQueryOver(queryOver);
        }

        public IList<TEntity> GetAllByQueryOver(QueryOver<TEntity> queryOver)
        {
            return Repository.GetAllByQueryOver(queryOver);
        }
    }
}
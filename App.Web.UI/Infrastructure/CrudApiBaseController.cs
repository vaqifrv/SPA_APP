using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using App.Core.Infrastructure;
using App.Core;
using App.Services.Services;
using App.Repository.Models.Messages;

namespace App.Web.UI.Infrastructure
{
    public class CrudApiBaseController<TEntity, TEntityId, TService> : ApiController 
        where TEntity:EntityBase<TEntityId>
        where TService:IServiceBase<TEntity, TEntityId>
    {
        protected TService Service;

        public CrudApiBaseController()
        {
            this.Service = IoC.Current.GetObject<TService>();
        }

        public CrudApiBaseController(TService service)
        {
            this.Service = service;
        }

        // GET: api/GuidApiBase
        public ObjectWrapperWithNameOfSet Get(ListRequest<TEntity> request)
        {
            
            ListResponse<TEntity> response = Service.List(request);
            GenerateExceptionIfNotSuccess(response);

            return new ObjectWrapperWithNameOfSet("list", response.List);
        }

        // GET: api/GuidApiBase/5
        public TEntity Get(TEntityId id)
        {
            ValueResponse<TEntity> response = Service.GetItem(id);
            GenerateExceptionIfNotSuccess(response);

            return response.Value;
        }

        // POST: api/GuidApiBase
        public string Post([FromBody]TEntity value)
        {
            ValueResponse<TEntity> response = Service.Update(new ValueRequest<TEntity>
            {
                Value = value
            });

            GenerateExceptionIfNotSuccess(response);
            return response.Value.Id.ToString();
        }

        // PUT: api/GuidApiBase/5
        public void Put(TEntityId id, [FromBody]TEntity value)
        {
            ValueResponse<TEntity> response = Service.Update(new ValueRequest<TEntity>
            {
                Value = value
            });

            GenerateExceptionIfNotSuccess(response);
        }

        // DELETE: api/GuidApiBase/5
        public void Delete(TEntityId id)
        {
            ResponseBase response = Service.Delete(id);
            GenerateExceptionIfNotSuccess(response);
        }

        protected void GenerateExceptionIfNotSuccess(ResponseBase response)
        {
            if (!response.Success)
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(JsonConvert.SerializeObject(response.Errors), System.Text.UnicodeEncoding.Unicode, "application/json")
                });
        }

    }
}

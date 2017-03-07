using App.Core;
using App.Services.Services;

namespace App.Web.UI.Infrastructure
{
    public class GuidApiController<TEntity, TService> : CrudApiBaseController<TEntity, string, TService>
        where TEntity:GuidEntityBase
        where TService:IGuidServiceBase<TEntity>
    {

    }
}

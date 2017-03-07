using App.Core;

namespace App.Services.Services
{
    public class GuidServiceBase<TEntity> : ServiceBase<TEntity, string>, IGuidServiceBase<TEntity>
        where TEntity : GuidEntityBase
    {
    }
}
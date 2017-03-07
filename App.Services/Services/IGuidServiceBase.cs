using App.Core;

namespace App.Services.Services
{
    public interface IGuidServiceBase<TEntity> : IServiceBase<TEntity, string> where TEntity : GuidEntityBase
    {
    }
}
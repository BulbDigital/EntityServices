using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class ReadOnlyEntityServiceAsync<TEntity, TDto> : ReadOnlyEntityServiceAsync<DbContext, TEntity, TDto>, IReadOnlyEntityServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public ReadOnlyEntityServiceAsync(IEntityCrudServiceAsync<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class ReadOnlyEntityServiceAsync<TContext, TEntity, TDto> : StatusGenericHandler, IReadOnlyEntityServiceAsync<TContext, TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TContext : DbContext
    {
        protected readonly IEntityCrudServiceAsync<TContext, TEntity> EntityCrudService;
        public ReadOnlyEntityServiceAsync(IEntityCrudServiceAsync<TContext, TEntity> entityCrudService)
        {
            EntityCrudService = entityCrudService;
        }

        public virtual IQueryable<TDto> Get()
        {
            var result = EntityCrudService.ProjectFromEntityToDto<TDto>(e => e);
            CombineStatuses(EntityCrudService);
            return result;
        }

        #region Async Methods
        public virtual async Task<TDto> GetSingleAsync(params object[] keys)
        {
            var result = await EntityCrudService.ReadSingleAsync<TDto>(keys);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual async Task<TDto> GetSingleAsync(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = await EntityCrudService.ReadSingleAsync(whereExpression);
            CombineStatuses(EntityCrudService);
            return result;
        }
        #endregion
    }
}

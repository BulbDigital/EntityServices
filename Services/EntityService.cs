using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class EntityService<TEntity, TDto> : EntityService<DbContext, TEntity, TDto, TDto, TDto>, IEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public EntityService(IEntityCrudService<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class EntityService<TEntity, TDto, TCreateDto, TUpdateDto> : EntityService<DbContext, TEntity, TDto, TCreateDto, TUpdateDto>, IEntityService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        public EntityService(IEntityCrudService<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class EntityService<TContext, TEntity, TDto> : EntityService<TContext, TEntity, TDto, TDto, TDto>, IEntityService<TEntity, TDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public EntityService(IEntityCrudService<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class EntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : StatusGenericHandler, IEntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        protected readonly IEntityCrudService<TContext, TEntity> EntityCrudService;
        public EntityService(IEntityCrudService<TContext, TEntity> entityCrudService)
        {
            EntityCrudService = entityCrudService;
        }

        public virtual IQueryable<TDto> Get()
        {
            var result = EntityCrudService.ReadManyNoTracked<TDto>();
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual async Task<TDto> GetSingleAsync(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = await EntityCrudService.ReadSingleAsync(whereExpression);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual async Task<TDto> GetSingleAsync(params object[] keys)
        {
            var result = await EntityCrudService.ReadSingleAsync<TDto>(keys);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual async Task<TCreateDto> CreateAsync(TCreateDto createDto)
        {
            var newEntity = await EntityCrudService.CreateAndSaveAsync(createDto);
            CombineStatuses(EntityCrudService);
            return newEntity;
        }

        public virtual async Task<TUpdateDto> UpdateAsync(TUpdateDto updateDto)
        {
            await EntityCrudService.UpdateAndSaveAsync(updateDto, "AutoMapper");
            CombineStatuses(EntityCrudService);
            return updateDto;
        }

        public virtual async Task DeleteAsync(params object[] keys)
        {
            await EntityCrudService.DeleteAndSaveAsync(keys);
            CombineStatuses(EntityCrudService);
        }

        protected virtual async Task ExecuteAsync(TDto dto, Expression<Action<TEntity>> expression)
        {
            await EntityCrudService.UpdateAndSaveAsync(dto, (expression.Body as MethodCallExpression).Method.Name);
        }
    }
}

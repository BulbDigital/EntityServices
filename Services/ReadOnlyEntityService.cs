﻿using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class ReadOnlyEntityService<TEntity, TDto> : EntityService<DbContext, TEntity, TDto>, IReadOnlyEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public ReadOnlyEntityService(IEntityCrudService<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class ReadOnlyEntityService<TContext, TEntity, TDto> : StatusGenericHandler, IReadOnlyEntityService<TContext, TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TContext : DbContext
    {
        protected readonly IEntityCrudService<TContext, TEntity> EntityCrudService;
        public ReadOnlyEntityService(IEntityCrudService<TContext, TEntity> entityCrudService)
        {
            EntityCrudService = entityCrudService;
        }

        public virtual IQueryable<TDto> Get()
        {
            var result = EntityCrudService.ProjectFromEntityToDto<TDto>(e => e);
            CombineStatuses(EntityCrudService);
            return result;
        }

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
    }
}

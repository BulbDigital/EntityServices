using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class EntityServiceAsync<TEntity, TDto, TCreateDto, TUpdateDto> : EntityServiceAsync<DbContext, TEntity, TDto, TCreateDto, TUpdateDto>, IEntityServiceAsync<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        public EntityServiceAsync(IEntityCrudServiceAsync<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class EntityServiceAsync<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : ActionOnlyEntityServiceAsync<TContext, TEntity, TDto, TCreateDto>, IEntityServiceAsync<TContext, TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        public EntityServiceAsync(IEntityCrudServiceAsync<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {}

        #region Async Method
        public virtual async Task<TUpdateDto> UpdateAsync(TUpdateDto updateDto, string method = "AutoMapper")
        {
            if(!Validate(updateDto))
            {
                return null;
            }

            await EntityCrudService.UpdateAndSaveAsync(updateDto);
            CombineStatuses(EntityCrudService);
            return updateDto;
        }
        #endregion
    }
}

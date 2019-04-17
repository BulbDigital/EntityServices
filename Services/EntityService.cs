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

    public class EntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : ActionOnlyEntityService<TContext, TEntity, TDto, TCreateDto> , IEntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        public EntityService(IEntityCrudService<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {}

        #region Sync Method
        public virtual TUpdateDto Update(TUpdateDto updateDto, string method = "AutoMapper")
        {
            if (!Validate(updateDto))
            {
                return null;
            }

            EntityCrudService.UpdateAndSave(updateDto);
            CombineStatuses(EntityCrudService);
            return updateDto;
        }

        #endregion
    }
}

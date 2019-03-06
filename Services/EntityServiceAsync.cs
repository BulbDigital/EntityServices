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
    public class EntityServiceAsync<TEntity, TDto> : EntityServiceAsync<DbContext, TEntity, TDto, TDto, TDto>, IEntityServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public EntityServiceAsync(IEntityCrudServiceAsync<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

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

    public class EntityServiceAsync<TContext, TEntity, TDto> : EntityServiceAsync<TContext, TEntity, TDto, TDto, TDto>, IEntityServiceAsync<TEntity, TDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        public EntityServiceAsync(IEntityCrudServiceAsync<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class EntityServiceAsync<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : StatusGenericHandler, IEntityServiceAsync<TContext, TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
    {
        protected readonly IEntityCrudServiceAsync<TContext, TEntity> EntityCrudService;
        public EntityServiceAsync(IEntityCrudServiceAsync<TContext, TEntity> entityCrudService)
        {
            EntityCrudService = entityCrudService;
        }

        public virtual IQueryable<TDto> Get()
        {
            var result = EntityCrudService.ReadManyNoTracked<TDto>();
            CombineStatuses(EntityCrudService);
            return result;
        }

        #region Async Method
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
            if (!Validate(createDto))
            {
                return null;
            }

            var newEntity = await EntityCrudService.CreateAndSaveAsync(createDto);
            CombineStatuses(EntityCrudService);
            return newEntity;
        }

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

        public virtual async Task DeleteAsync(params object[] keys)
        {
            await EntityCrudService.DeleteAndSaveAsync(keys);
            CombineStatuses(EntityCrudService);
        }

        protected virtual async Task ExecuteAsync<TExecuteDto>(TExecuteDto dto, Expression<Action<TEntity>> expression)
            where TExecuteDto : class, ILinkToEntity<TEntity>
        {
            if (!Validate(dto))
            {
                return;
            }

            await EntityCrudService.UpdateAndSaveAsync(dto, (expression.Body as MethodCallExpression).Method.Name);
        }
        #endregion


        public bool Validate(object @object)
        {
            var context = new ValidationContext(@object, serviceProvider: null, items: null);
            var validation = new List<ValidationResult>();
            var result = Validator.TryValidateObject(
                @object, context, validation,
                validateAllProperties: true
            );

            if(validation.Count() > 0)
            {
                AddValidationResults(validation);
            }

            return result;
        }
    }
}

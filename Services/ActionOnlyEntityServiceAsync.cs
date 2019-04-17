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
    public class ActionOnlyEntityServiceAsync<TEntity, TDto, TCreateDto> : ActionOnlyEntityServiceAsync<DbContext, TEntity, TDto, TCreateDto>, IActionOnlyEntityServiceAsync<TEntity, TDto, TCreateDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        public ActionOnlyEntityServiceAsync(IEntityCrudServiceAsync<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class ActionOnlyEntityServiceAsync<TContext, TEntity, TDto, TCreateDto> : ReadOnlyEntityServiceAsync<TContext, TEntity, TDto>, IActionOnlyEntityServiceAsync<TContext, TEntity, TDto, TCreateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        public ActionOnlyEntityServiceAsync(IEntityCrudServiceAsync<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {}

        #region Async Method
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

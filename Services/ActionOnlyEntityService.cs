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
    public class ActionOnlyEntityService<TEntity, TDto, TCreateDto> : ActionOnlyEntityService<DbContext, TEntity, TDto, TCreateDto>, IActionOnlyEntityService<TEntity, TDto, TCreateDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        public ActionOnlyEntityService(IEntityCrudService<DbContext, TEntity> entityCrudService) : base(entityCrudService)
        {
        }
    }

    public class ActionOnlyEntityService<TContext, TEntity, TDto, TCreateDto> : ReadOnlyEntityService<TContext, TEntity, TDto>, IActionOnlyEntityService<TContext, TEntity, TDto, TCreateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        public ActionOnlyEntityService(IEntityCrudService<TContext, TEntity> entityCrudService) : base(entityCrudService)
        {}

        #region Sync Method
        public virtual TCreateDto Create(TCreateDto createDto)
        {
            if (!Validate(createDto))
            {
                return null;
            }

            var newEntity = EntityCrudService.CreateAndSave(createDto);
            CombineStatuses(EntityCrudService);
            return newEntity;
        }

        public virtual void Delete(params object[] keys)
        {
            EntityCrudService.DeleteAndSave(keys);
            CombineStatuses(EntityCrudService);
        }

        protected virtual void Execute<TExecuteDto>(TExecuteDto dto, Expression<Action<TEntity>> expression)
            where TExecuteDto : class, ILinkToEntity<TEntity>
        {
            if (!Validate(dto))
            {
                return;
            }

            EntityCrudService.UpdateAndSave(dto, (expression.Body as MethodCallExpression).Method.Name);
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

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

        public virtual IQueryable<TGetDto> Get<TGetDto>()
             where TGetDto : class, ILinkToEntity<TEntity>
        {
            var result = EntityCrudService.ReadManyNoTracked<TGetDto>();
            CombineStatuses(EntityCrudService);
            return result;
        }

        #region Sync Method
        public virtual TDto GetSingle(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = EntityCrudService.ReadSingle(whereExpression);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual TGetDto GetSingle<TGetDto>(Expression<Func<TGetDto, bool>> whereExpression)
             where TGetDto : class, ILinkToEntity<TEntity>
        {
            var result = EntityCrudService.ReadSingle(whereExpression);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual TDto GetSingle(params object[] keys)
        {
            var result = EntityCrudService.ReadSingle<TDto>(keys);
            CombineStatuses(EntityCrudService);
            return result;
        }

        public virtual TGetDto GetSingle<TGetDto>(params object[] keys)
             where TGetDto : class, ILinkToEntity<TEntity>
        {
            var result = EntityCrudService.ReadSingle<TGetDto>(keys);
            CombineStatuses(EntityCrudService);
            return result;
        }

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

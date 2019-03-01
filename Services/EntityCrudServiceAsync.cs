using GenericServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class EntityCrudServiceAsync<TEntity> : EntityCrudServiceAsync<DbContext, TEntity>, IEntityCrudServiceAsync<TEntity>
        where TEntity : class
    {
        public EntityCrudServiceAsync(ICrudServices<DbContext> crudServices, ICrudServicesAsync<DbContext> crudServicesAsync) : base(crudServices, crudServicesAsync)
        {
        }
    }

    public class EntityCrudServiceAsync<TContext, TEntity> : StatusGenericHandler, IEntityCrudServiceAsync<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly ICrudServicesAsync<TContext> CrudServicesAsync;
        public DbContext Context => CrudServicesAsync.Context;

        public EntityCrudServiceAsync(ICrudServices<TContext> crudServices, ICrudServicesAsync<TContext> crudServicesAsync)
        {
            CrudServicesAsync = crudServicesAsync;
        }

        #region Async Methods
        async Task<TDto> IEntityCrudServiceAsync<TEntity>.ReadSingleAsync<TDto>(params object[] keys)
        {
            var result = await CrudServicesAsync.ReadSingleAsync<TDto>(keys);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public async Task<TEntity> ReadSingleAsync(params object[] keys)
        {
            var result = await CrudServicesAsync.ReadSingleAsync<TEntity>(keys);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        async Task<TDto> IEntityCrudServiceAsync<TEntity>.ReadSingleAsync<TDto>(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = await CrudServicesAsync.ReadSingleAsync(whereExpression);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public async Task<TEntity> ReadSingleAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = await CrudServicesAsync.ReadSingleAsync(whereExpression);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        async Task<TDto> IEntityCrudServiceAsync<TEntity>.CreateAndSaveAsync<TDto>(TDto entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = await CrudServicesAsync.CreateAndSaveAsync<TDto>(entityOrDto, ctorOrStaticMethodName);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public async Task<TEntity> CreateAndSaveAsync(TEntity entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = await CrudServicesAsync.CreateAndSaveAsync(entityOrDto, ctorOrStaticMethodName);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        async Task IEntityCrudServiceAsync<TEntity>.UpdateAndSaveAsync<TDto>(TDto entityOrDto, string methodName = null)
        {
            await CrudServicesAsync.UpdateAndSaveAsync(entityOrDto, methodName);
            CombineStatuses(CrudServicesAsync);
        }

        public async Task UpdateAndSaveAsync(TEntity entityOrDto, string methodName = null)
        {
            await CrudServicesAsync.UpdateAndSaveAsync(entityOrDto, methodName);
            CombineStatuses(CrudServicesAsync);
        }

        public async Task<TEntity> UpdateAndSaveAsync(JsonPatchDocument<TEntity> patch, params object[] keys)
        {
            var result = await CrudServicesAsync.UpdateAndSaveAsync(patch, keys);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public async Task<TEntity> UpdateAndSaveAsync(JsonPatchDocument<TEntity> patch, Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = await CrudServicesAsync.UpdateAndSaveAsync(patch, whereExpression);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public async Task DeleteAndSaveAsync(params object[] keys)
        {
            await CrudServicesAsync.DeleteAndSaveAsync<TEntity>(keys);
            CombineStatuses(CrudServicesAsync);
        }

        public async Task DeleteWithActionAndSaveAsyn(Func<DbContext, TEntity, Task<IStatusGeneric>> runBeforeDelete, params object[] keys)
        {
            await CrudServicesAsync.DeleteAndSaveAsync<TEntity>(runBeforeDelete, keys);
            CombineStatuses(CrudServicesAsync);
        }
        #endregion


        IQueryable<TDto> IEntityCrudServiceAsync<TEntity>.ReadManyNoTracked<TDto>()
        {
            var result = CrudServicesAsync.ReadManyNoTracked<TDto>();
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        public IQueryable<TEntity> ReadManyNoTracked()
        {
            var result = CrudServicesAsync.ReadManyNoTracked<TEntity>();
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        IQueryable<TDto> IEntityCrudServiceAsync<TEntity>.ProjectFromEntityToDto<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            var result = CrudServicesAsync.ProjectFromEntityToDto<TEntity, TDto>(query);
            CombineStatuses(CrudServicesAsync);
            return result;
        }

        IQueryable<TEntity> IEntityCrudServiceAsync<TEntity>.ProjectFromDtoToEntity<TDto>(Func<IQueryable<TDto>, IQueryable<TDto>> query)
        {
            var result = CrudServicesAsync.ProjectFromEntityToDto<TDto, TEntity>(query);
            CombineStatuses(CrudServicesAsync);
            return result;
        }
    }
}

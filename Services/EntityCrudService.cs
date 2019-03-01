using GenericServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public class EntityCrudService<TEntity> : EntityCrudService<DbContext, TEntity>, IEntityCrudService<TEntity>
        where TEntity : class
    {
        public EntityCrudService(ICrudServices<DbContext> crudServices, ICrudServicesAsync<DbContext> crudServicesAsync) : base(crudServices, crudServicesAsync)
        {
        }
    }

    public class EntityCrudService<TContext, TEntity> : StatusGenericHandler, IEntityCrudService<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly ICrudServicesAsync<TContext> CrudServicesAsync;
        protected readonly ICrudServices<TContext> CrudServices;
        public DbContext Context => CrudServicesAsync.Context;

        public EntityCrudService(ICrudServices<TContext> crudServices, ICrudServicesAsync<TContext> crudServicesAsync)
        {
            CrudServices = crudServices;
            CrudServicesAsync = crudServicesAsync;
        }

        #region Async Methods
        async Task<TDto> IEntityCrudService<TEntity>.ReadSingleAsync<TDto>(params object[] keys)
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

        async Task<TDto> IEntityCrudService<TEntity>.ReadSingleAsync<TDto>(Expression<Func<TDto, bool>> whereExpression)
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

        async Task<TDto> IEntityCrudService<TEntity>.CreateAndSaveAsync<TDto>(TDto entityOrDto, string ctorOrStaticMethodName = null)
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

        async Task IEntityCrudService<TEntity>.UpdateAndSaveAsync<TDto>(TDto entityOrDto, string methodName = null)
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

        #region Sync Methods
        TDto IEntityCrudService<TEntity>.ReadSingle<TDto>(params object[] keys)
        {
            var result = CrudServices.ReadSingle<TDto>(keys);
            CombineStatuses(CrudServices);
            return result;
        }

        public TEntity ReadSingle(params object[] keys)
        {
            var result = CrudServices.ReadSingle<TEntity>(keys);
            CombineStatuses(CrudServices);
            return result;
        }

        TDto IEntityCrudService<TEntity>.ReadSingle<TDto>(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = CrudServices.ReadSingle(whereExpression);
            CombineStatuses(CrudServices);
            return result;
        }

        public TEntity ReadSingle(Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = CrudServices.ReadSingle(whereExpression);
            CombineStatuses(CrudServices);
            return result;
        }

        IQueryable<TDto> IEntityCrudService<TEntity>.ReadManyNoTracked<TDto>()
        {
            var result = CrudServices.ReadManyNoTracked<TDto>();
            CombineStatuses(CrudServices);
            return result;
        }

        public IQueryable<TEntity> ReadManyNoTracked()
        {
            var result = CrudServices.ReadManyNoTracked<TEntity>();
            CombineStatuses(CrudServices);
            return result;
        }

        IQueryable<TDto> IEntityCrudService<TEntity>.ProjectFromEntityToDto<TDto>(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            var result = CrudServices.ProjectFromEntityToDto<TEntity, TDto>(query);
            CombineStatuses(CrudServices);
            return result;
        }

        IQueryable<TEntity> IEntityCrudService<TEntity>.ProjectFromDtoToEntity<TDto>(Func<IQueryable<TDto>, IQueryable<TDto>> query)
        {
            var result = CrudServices.ProjectFromEntityToDto<TDto, TEntity>(query);
            CombineStatuses(CrudServices);
            return result;
        }

        TDto IEntityCrudService<TEntity>.CreateAndSave<TDto>(TDto entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = CrudServices.CreateAndSave(entityOrDto, ctorOrStaticMethodName);
            CombineStatuses(CrudServices);
            return result;
        }

        public TEntity CreateAndSave(TEntity entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = CrudServices.CreateAndSave(entityOrDto, ctorOrStaticMethodName);
            CombineStatuses(CrudServices);
            return result;
        }

        void IEntityCrudService<TEntity>.UpdateAndSave<TDto>(TDto entityOrDto, string methodName = null)
        {
            CrudServices.UpdateAndSave(entityOrDto, methodName);
            CombineStatuses(CrudServices);
        }

        public void UpdateAndSave(TEntity entityOrDto, string methodName = null)
        {
            CrudServices.UpdateAndSave(entityOrDto, methodName);
            CombineStatuses(CrudServices);
        }

        public TEntity UpdateAndSave(JsonPatchDocument<TEntity> patch, params object[] keys)
        {
            var result = CrudServices.UpdateAndSave(patch, keys);
            CombineStatuses(CrudServices);
            return result;
        }

        public TEntity UpdateAndSave(JsonPatchDocument<TEntity> patch, Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = CrudServices.UpdateAndSave(patch, whereExpression);
            CombineStatuses(CrudServices);
            return result;
        }

        public void DeleteAndSave(params object[] keys)
        {
            CrudServices.DeleteAndSave<TEntity>(keys);
            CombineStatuses(CrudServices);
        }

        public void DeleteWithActionAndSave(Func<DbContext, TEntity, Task<IStatusGeneric>> runBeforeDelete, params object[] keys)
        {
            CrudServices.DeleteAndSave<TEntity>(runBeforeDelete, keys);
            CombineStatuses(CrudServices);
        }
        #endregion
    }
}

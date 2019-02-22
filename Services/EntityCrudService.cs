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
        public EntityCrudService(ICrudServicesAsync<DbContext> crudServices) : base(crudServices)
        {
        }
    }

    public class EntityCrudService<TContext, TEntity> : StatusGenericHandler, IEntityCrudService<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly ICrudServicesAsync<TContext> CrudServices;
        public DbContext Context => CrudServices.Context;

        public EntityCrudService(ICrudServicesAsync<TContext> crudServices)
        {
            this.CrudServices = crudServices;
        }

        async Task<TDto> IEntityCrudService<TEntity>.ReadSingleAsync<TDto>(params object[] keys)
        {
            var result = await CrudServices.ReadSingleAsync<TDto>(keys);
            CombineStatuses(CrudServices);
            return result;
        }

        public async Task<TEntity> ReadSingleAsync(params object[] keys)
        {
            var result = await CrudServices.ReadSingleAsync<TEntity>(keys);
            CombineStatuses(CrudServices);
            return result;
        }

        async Task<TDto> IEntityCrudService<TEntity>.ReadSingleAsync<TDto>(Expression<Func<TDto, bool>> whereExpression)
        {
            var result = await CrudServices.ReadSingleAsync<TDto>(whereExpression);
            CombineStatuses(CrudServices);
            return result;
        }

        public async Task<TEntity> ReadSingleAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = await CrudServices.ReadSingleAsync<TEntity>(whereExpression);
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

        async Task<TDto> IEntityCrudService<TEntity>.CreateAndSaveAsync<TDto>(TDto entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = await CrudServices.CreateAndSaveAsync(entityOrDto, ctorOrStaticMethodName);
            CombineStatuses(CrudServices);
            return result;
        }

        public async Task<TEntity> CreateAndSaveAsync(TEntity entityOrDto, string ctorOrStaticMethodName = null)
        {
            var result = await CrudServices.CreateAndSaveAsync(entityOrDto);
            CombineStatuses(CrudServices);
            return result;
        }

        async Task IEntityCrudService<TEntity>.UpdateAndSaveAsync<TDto>(TDto entityOrDto, string methodName = null)
        {
            await CrudServices.UpdateAndSaveAsync(entityOrDto, methodName);
            CombineStatuses(CrudServices);
        }

        public async Task UpdateAndSaveAsync(TEntity entityOrDto, string methodName = null)
        {
            await CrudServices.UpdateAndSaveAsync(entityOrDto, methodName);
            CombineStatuses(CrudServices);
        }

        public async Task<TEntity> UpdateAndSaveAsync(JsonPatchDocument<TEntity> patch, params object[] keys)
        {
            var result = await CrudServices.UpdateAndSaveAsync(patch, keys);
            CombineStatuses(CrudServices);
            return result;
        }

        public async Task<TEntity> UpdateAndSaveAsync(JsonPatchDocument<TEntity> patch, Expression<Func<TEntity, bool>> whereExpression)
        {
            var result = await CrudServices.UpdateAndSaveAsync(patch, whereExpression);
            CombineStatuses(CrudServices);
            return result;
        }

        public async Task DeleteAndSaveAsync(params object[] keys)
        {
            await CrudServices.DeleteAndSaveAsync<TEntity>(keys);
            CombineStatuses(CrudServices);
        }

        public async Task DeleteWithActionAndSaveAsyn(Func<DbContext, TEntity, Task<IStatusGeneric>> runBeforeDelete, params object[] keys)
        {
            await CrudServices.DeleteAndSaveAsync<TEntity>(runBeforeDelete, keys);
            CombineStatuses(CrudServices);
        }
    }
}

using GenericServices;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
  public class ReadOnlyEntityService<TEntity, TDto> : ReadOnlyEntityService<DbContext, TEntity, TDto>, IReadOnlyEntityService<TEntity, TDto>
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

    public virtual IQueryable<TGetDto> Get<TGetDto>()
         where TGetDto : class, ILinkToEntity<TEntity>
    {
      var result = EntityCrudService.ReadManyNoTracked<TGetDto>();
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

  }
}

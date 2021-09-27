using GenericServices;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
  public interface IReadOnlyEntityService<TContext, TEntity, TDto> : IReadOnlyEntityService<TEntity, TDto>
      where TEntity : class
      where TDto : class, ILinkToEntity<TEntity>
      where TContext : DbContext
  {
  }

  public interface IReadOnlyEntityService<TEntity, TDto> : IStatusGeneric
      where TEntity : class
      where TDto : class, ILinkToEntity<TEntity>
  {
    IQueryable<TDto> Get();
    IQueryable<TDto> Get(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);
    TDto GetSingle(params object[] keys);
    TDto GetSingle(Expression<Func<TDto, bool>> whereExpression);
    IQueryable<TGetDto> Get<TGetDto>() where TGetDto : class, ILinkToEntity<TEntity>;
    TGetDto GetSingle<TGetDto>(params object[] keys) where TGetDto : class, ILinkToEntity<TEntity>;
    TGetDto GetSingle<TGetDto>(Expression<Func<TGetDto, bool>> whereExpression) where TGetDto : class, ILinkToEntity<TEntity>;
  }
}

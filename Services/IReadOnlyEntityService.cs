using GenericServices;
using Microsoft.EntityFrameworkCore;
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
        Task<TDto> GetSingleAsync(params object[] keys);
        Task<TDto> GetSingleAsync(Expression<Func<TDto, bool>> whereExpression);
        TDto GetSingle(params object[] keys);
        TDto GetSingle(Expression<Func<TDto, bool>> whereExpression);
    }
}

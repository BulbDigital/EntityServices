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
        IQueryable<TDto> Get(Func<IQueryable<TDto>, IQueryable<TDto>> query);
        IQueryable<TDto> Get();
        TDto GetSingle(Func<IQueryable<TDto>, IQueryable<TDto>> query);
        Task<TDto> GetSingleAsync(params object[] keys);
        Task<TDto> GetSingleAsync(Func<IQueryable<TDto>, bool> whereExpression);
    }
}

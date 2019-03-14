using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public interface IReadOnlyEntityServiceAsync<TContext, TEntity, TDto> : IReadOnlyEntityServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TContext : DbContext
    {
    }

    public interface IReadOnlyEntityServiceAsync<TEntity, TDto> : IStatusGeneric
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        IQueryable<TDto> Get();
        Task<TDto> GetSingleAsync(params object[] keys);
        Task<TDto> GetSingleAsync(Expression<Func<TDto, bool>> whereExpression);
        IQueryable<TGetDto> Get<TGetDto>() where TGetDto : class, ILinkToEntity<TEntity>;
        Task<TGetDto> GetSingleAsync<TGetDto>(params object[] keys) where TGetDto : class, ILinkToEntity<TEntity>;
        Task<TGetDto> GetSingleAsync<TGetDto>(Expression<Func<TGetDto, bool>> whereExpression) where TGetDto : class, ILinkToEntity<TEntity>;
    }
}

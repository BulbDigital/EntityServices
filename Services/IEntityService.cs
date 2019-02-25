using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityServices.Services
{
    public interface IEntityService<TContext, TEntity, TDto> : IEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TContext : DbContext
    {
    }

    public interface IEntityService<TEntity, TDto> : IStatusGeneric, IReadOnlyEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
        Task<TUpdate> UpdateAsync<TUpdate>(TUpdate updateDTO) where TUpdate : class, ILinkToEntity<TEntity>, new();
        Task<TCreate> CreateAsync<TCreate>(TCreate createDTO) where TCreate : class, ILinkToEntity<TEntity>, new();
        Task DeleteAsync(params object[] keys);
    }
}

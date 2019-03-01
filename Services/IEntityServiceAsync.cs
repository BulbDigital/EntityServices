using GenericServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EntityServices.Services
{

    public interface IEntityServiceAsync<TContext, TEntity, TDto> : IEntityServiceAsync<TEntity, TDto, TDto, TDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityServiceAsync<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : IEntityServiceAsync<TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityServiceAsync<TEntity, TDto> : IEntityServiceAsync<TEntity, TDto, TDto, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityServiceAsync<TEntity, TDto, TCreateDto, TUpdateDto> : IStatusGeneric, IReadOnlyEntityServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        Task<TUpdateDto> UpdateAsync(TUpdateDto updateDto, string method = "AutoMapper");
        Task<TCreateDto> CreateAsync(TCreateDto createDto);
        Task DeleteAsync(params object[] keys);
    }
}

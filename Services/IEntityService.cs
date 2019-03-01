using GenericServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EntityServices.Services
{

    public interface IEntityService<TContext, TEntity, TDto> : IEntityService<TEntity, TDto, TDto, TDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : IEntityService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityService<TEntity, TDto> : IEntityService<TEntity, TDto, TDto, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityService<TEntity, TDto, TCreateDto, TUpdateDto> : IStatusGeneric, IReadOnlyEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        TUpdateDto Update(TUpdateDto updateDto, string method = "AutoMapper");
        TCreateDto Create(TCreateDto createDto);
        void Delete(params object[] keys);

        Task<TUpdateDto> UpdateAsync(TUpdateDto updateDto, string method = "AutoMapper");
        Task<TCreateDto> CreateAsync(TCreateDto createDto);
        Task DeleteAsync(params object[] keys);
    }
}

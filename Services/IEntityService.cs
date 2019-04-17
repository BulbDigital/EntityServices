using GenericServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EntityServices.Services
{

    public interface IEntityService<TContext, TEntity, TDto, TCreateDto, TUpdateDto> : IEntityService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IEntityService<TEntity, TDto, TCreateDto, TUpdateDto> : IStatusGeneric, IActionOnlyEntityService<TEntity, TDto, TCreateDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TUpdateDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        TUpdateDto Update(TUpdateDto updateDto, string method = "AutoMapper");
    }
}

using GenericServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EntityServices.Services
{

    public interface IActionOnlyEntityService<TContext, TEntity, TDto, TCreateDto> : IActionOnlyEntityService<TEntity, TDto, TCreateDto>
        where TContext : DbContext
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
    }

    public interface IActionOnlyEntityService<TEntity, TDto, TCreateDto> : IStatusGeneric, IReadOnlyEntityService<TEntity, TDto>
        where TEntity : class
        where TDto : class, ILinkToEntity<TEntity>
        where TCreateDto : class, ILinkToEntity<TEntity>
    {
        TCreateDto Create(TCreateDto createDto);
        void Delete(params object[] keys);
    }
}

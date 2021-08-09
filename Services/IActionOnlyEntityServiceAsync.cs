using GenericBizRunner;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System.Threading.Tasks;

namespace EntityServices.Services
{


  public interface IActionOnlyEntityServiceAsync<TContext, TEntity, TDto, TCreateDto> : IActionOnlyEntityServiceAsync<TEntity, TDto, TCreateDto>
      where TContext : DbContext
      where TEntity : class
      where TDto : class, ILinkToEntity<TEntity>
      where TCreateDto : class, ILinkToEntity<TEntity>
  {
  }

  public interface IActionOnlyEntityServiceAsync<TEntity, TDto, TCreateDto> : IStatusGeneric, IReadOnlyEntityServiceAsync<TEntity, TDto>
      where TEntity : class
      where TDto : class, ILinkToEntity<TEntity>
      where TCreateDto : class, ILinkToEntity<TEntity>
  {
    Task<TCreateDto> CreateAsync(TCreateDto createDto);
    Task DeleteAsync(params object[] keys);
  }
}

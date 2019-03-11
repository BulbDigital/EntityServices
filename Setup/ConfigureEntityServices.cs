using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EntityServices.Setup
{
    public static class ConfigureEntityServices
    {
        public static IServiceCollection EntityServicesSimpleSetup<TContext>(this IServiceCollection services,
            params Assembly[] assembliesToScan) where TContext : DbContext
        {
            services.AddTransient(typeof(Services.IEntityCrudService<,>), typeof(Services.EntityCrudService<,>));
            services.AddTransient(typeof(Services.IEntityCrudServiceAsync<,>), typeof(Services.EntityCrudServiceAsync<,>));

            return services.ConfigureGenericServicesEntities(typeof(TContext))
                .ScanAssemblesForDtos(assembliesToScan)
                .RegisterGenericServices(typeof(TContext));
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.AspNetCore.Infrastructure.DependencyInjection;

/// <summary>
/// Register AutoMapper dependencies.
/// </summary>
public class AutoMapperModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SearchEntitiesQuery).Assembly);
    }
}

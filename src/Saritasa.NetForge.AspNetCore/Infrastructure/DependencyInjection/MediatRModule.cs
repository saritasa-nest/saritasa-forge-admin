using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.AspNetCore.Infrastructure.DependencyInjection;

/// <summary>
/// Register Mediator as dependency.
/// </summary>
internal static class MediatRModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SearchEntitiesQuery).Assembly));
    }
}

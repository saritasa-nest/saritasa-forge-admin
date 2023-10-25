using Saritasa.NetForge.Mvvm.ViewModels;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;

/// <summary>
/// Register AutoMapper dependencies.
/// </summary>
internal static class AutoMapperModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        var useCasesAssembly = typeof(SearchEntitiesQuery).Assembly;
        var viewModelsAssembly = typeof(BaseViewModel).Assembly;

        services.AddAutoMapper(useCasesAssembly, viewModelsAssembly);
    }
}

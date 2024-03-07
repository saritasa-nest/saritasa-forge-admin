using MudBlazor.Services;
using Saritasa.NetForge.Blazor.Infrastructure.Navigation;
using Saritasa.NetForge.Blazor.Infrastructure.Services;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Services;

namespace Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;

/// <summary>
/// Application specific dependencies.
/// </summary>
internal static class ApplicationModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ViewModelFactory>();

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices();
        services.AddMemoryCache();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IEntityService, EntityService>();
        services.AddTransient<IFileService, FileService>();
    }
}

using MudBlazor.Services;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Services;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.Navigation;
using Saritasa.NetForge.Infrastructure.Services;
using Saritasa.NetForge.MVVM.Navigation;
using Saritasa.NetForge.MVVM.ViewModels;

namespace Saritasa.NetForge.Infrastructure.DependencyInjection;

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
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PreventDuplicates = false;
        });
        services.AddMemoryCache();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IEntityService, EntityService>();
        services.AddTransient<IFileService, FileService>();
    }
}

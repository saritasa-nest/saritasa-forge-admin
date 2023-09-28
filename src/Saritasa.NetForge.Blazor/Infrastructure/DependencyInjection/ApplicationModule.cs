using MudBlazor.Services;
using Saritasa.NetForge.Blazor.Infrastructure.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels;

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
    }
}

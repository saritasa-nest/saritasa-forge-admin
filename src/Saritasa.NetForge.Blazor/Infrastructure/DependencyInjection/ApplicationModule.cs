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
    }
}

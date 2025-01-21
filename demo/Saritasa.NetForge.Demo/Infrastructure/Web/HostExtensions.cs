namespace Saritasa.NetForge.Demo.Infrastructure.Web;

/// <summary>
/// Contains extensions for <see cref="WebApplication"/> class.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Run application and starts initialization.
    /// See motivation of this workflow here: https://wiki.saritasa.rocks/general/rest-api-guidelines/#health-check.
    /// </summary>
    /// <param name="host">Host to run.</param>
    public static async Task RunAndInit(this IHost host)
    {
        var initializationStatus = host.Services.GetRequiredService<AppInitializationStatusStorage>();
        await host.StartAsync();

        await host.InitAsync();

        initializationStatus.HasAppInitialized = true;

        await host.WaitForShutdownAsync();
    }
}

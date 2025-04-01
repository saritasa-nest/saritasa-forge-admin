using Microsoft.Extensions.DependencyInjection.Extensions;
using Saritasa.NetForge.Demo.Infrastructure.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

/// <summary>
/// Helpers class for registering ephemeral SQLite connection.
/// </summary>
public static class EphemeralSqliteModule
{
    /// <summary>
    /// Register ephemeral SQLite services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static void AddEphemeralSqlite(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        // Stores the migrated and seeded database snapshot's location.
        // Automatically delete it after the application is closed.
        services.TryAddSingleton<DbSnapshot>();

        // This make sure that all DbConnection reference the same file,
        // and the SQLite file will be cleaned up when the scope ends.
        services.TryAddScoped<EphemeralSqliteConnectionFactory>();
        services.TryAddTransient<IEphemeralSqliteConnectionFactory>(static sp =>
        {
            // This is required because the Demo app's SP
            // and NetForge's SP are different, for some reason.
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext != null)
            {
                sp = httpContext.RequestServices;
            }

            return sp.GetRequiredService<EphemeralSqliteConnectionFactory>();
        });
    }
}
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

        // Stores the database snapshot's location.
        // Automatically delete it after the application is closed.
        services.TryAddSingleton<DbSnapshot>();

        services.TryAddScoped<IEphemeralSqliteConnectionFactory, EphemeralSqliteConnectionFactory>();
    }
}
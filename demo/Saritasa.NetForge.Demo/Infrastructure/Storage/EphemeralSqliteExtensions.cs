using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Extension methods for Ephemeral SQLite.
/// </summary>
public static class EphemeralSqliteExtensions
{
    /// <summary>
    /// Use Ephemeral SQLite.
    /// </summary>
    /// <param name="optionsBuilder">Options builder.</param>
    public static DbContextOptionsBuilder UseEphemeralSqlite(this DbContextOptionsBuilder optionsBuilder)
    {
        var extension = new EphemeralSqliteOptionsExtension();
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        // Connection string does not matter since we will be overriding the connection provider
        return optionsBuilder.UseSqlite(connectionString: string.Empty);
    }
}
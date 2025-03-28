using System.Data.Common;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Represent a service that create ephemeral SQLite connection.
/// </summary>
internal interface IEphemeralSqliteConnectionFactory : IDisposable
{
    /// <summary>
    /// Create database connection.
    /// </summary>
    /// <returns>Database connection.</returns>
    DbConnection CreateConnection();

    /// <summary>
    /// Dump the database's content.
    /// </summary>
    /// <param name="destination">Dump destination.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DumpDatabase(string destination, CancellationToken cancellationToken);

    /// <summary>
    /// Load the database's content.
    /// </summary>
    /// <param name="source">Dump source.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task LoadDatabase(string source, CancellationToken cancellationToken);
}
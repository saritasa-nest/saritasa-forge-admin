using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Microsoft.Data.Sqlite;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <inheritdoc cref="IEphemeralSqliteConnectionFactory" />
/// <remarks>
/// Hold the reference to the actual SQLite database.
/// Will automatically remove it when the scope ends.
/// </remarks>
internal class EphemeralSqliteConnectionFactory : CriticalFinalizerObject, IEphemeralSqliteConnectionFactory
{
    private readonly string sqliteDbPath = FileHelpers.CreateTemporaryFileSecure(".sqlite");
    private bool disposed;

    private static SqliteConnection CreateConnection(string path)
    {
        return new SqliteConnection($"Data Source={path};Pooling=false");
    }

    /// <inheritdoc />
    DbConnection IEphemeralSqliteConnectionFactory.CreateConnection() => CreateConnection(sqliteDbPath);

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing">Whether this method is called by <see cref="Dispose()"/> or not.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        try
        {
            File.Delete(sqliteDbPath);
        }
        finally
        {
            disposed = true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static async Task BackupDatabase(string targetPath, string destinationPath, CancellationToken cancellationToken)
    {
        await using var targetConnection = CreateConnection(targetPath);
        await targetConnection.OpenAsync(cancellationToken);
        await using var destinationConnection = CreateConnection(destinationPath);
        targetConnection.BackupDatabase(destinationConnection);
    }

    /// <inheritdoc />
    public Task DumpDatabase(string destination, CancellationToken cancellationToken)
    {
        return BackupDatabase(sqliteDbPath, destination, cancellationToken);
    }

    /// <inheritdoc />
    public Task LoadDatabase(string source, CancellationToken cancellationToken)
    {
        // We could just copy and paste the file
        // but this would prevent edge cases.
        return BackupDatabase(source, sqliteDbPath, cancellationToken);
    }

    /// <inheritdoc />
    ~EphemeralSqliteConnectionFactory()
    {
        Dispose(false);
    }
}
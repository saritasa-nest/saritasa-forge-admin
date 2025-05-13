using System.Collections.Concurrent;
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
    private static ConcurrentDictionary<Guid, string> SqliteDbPathStores { get; } = new();
    
    private readonly string sqliteDbPath;
    private bool disposed;

    private static SqliteConnection CreateConnection(string path)
    {
        return new SqliteConnection($"Data Source={path};Pooling=false");
    }
    
    private static string GetNewDbPath() => FileHelpers.CreateTemporaryFileSecure(".sqlite");

    /// <summary>
    /// Constructor.
    /// </summary>
    public EphemeralSqliteConnectionFactory(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null ||
            !httpContextAccessor.HttpContext.Items.TryGetValue(SessionContext.HttpContextKey, out var item) ||
            item is not SessionContext sessionContext)
        {
            sqliteDbPath = GetNewDbPath();
            return;
        }

        sqliteDbPath = SqliteDbPathStores.GetOrAdd(sessionContext.SessionId, static (sessionId, cancellationToken) =>
        {
            var newDbPath = GetNewDbPath();
            cancellationToken.Register(() =>
            {
                File.Delete(newDbPath);
                SqliteDbPathStores.Remove(sessionId, out _);
            });
            return newDbPath;
        }, sessionContext.CancellationToken);
        disposed = true;
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

    private static async Task BackupDatabase(string sourcePath, string destinationPath, CancellationToken cancellationToken)
    {
        await using var sourceConnection = CreateConnection(sourcePath);
        await sourceConnection.OpenAsync(cancellationToken);
        await using var destinationConnection = CreateConnection(destinationPath);
        sourceConnection.BackupDatabase(destinationConnection);
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
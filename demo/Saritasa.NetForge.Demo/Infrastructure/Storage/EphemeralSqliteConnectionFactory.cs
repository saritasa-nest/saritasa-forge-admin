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
    private static ConcurrentDictionary<HttpContext, string> SqliteDbPathStores { get; } = new();
    
    private readonly string sqliteDbPath;
    private bool disposed;

    private static SqliteConnection CreateConnection(string path)
    {
        return new SqliteConnection($"Data Source={path};Pooling=false");
    }
    
    private static string GetNewDbPath() => FileHelpers.CreateTemporaryFileSecure(".sqlite");

    public EphemeralSqliteConnectionFactory(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            sqliteDbPath = GetNewDbPath();
            return;
        }

        var cancellationToken = httpContext.RequestAborted;
        if (!cancellationToken.CanBeCanceled)
        {
            sqliteDbPath = GetNewDbPath();
            return;
        }
        
        sqliteDbPath = SqliteDbPathStores.GetOrAdd(httpContext, static _ => GetNewDbPath());
        cancellationToken.Register(() =>
        {
            File.Delete(sqliteDbPath);
            SqliteDbPathStores.Remove(httpContext, out _);
        });
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
        await using var targetConnection = CreateConnection(sourcePath);
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
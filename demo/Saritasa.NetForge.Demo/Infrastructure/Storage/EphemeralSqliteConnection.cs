using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Microsoft.Data.Sqlite;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <inheritdoc cref="IEphemeralSqliteConnection" />
internal class EphemeralSqliteConnection : CriticalFinalizerObject, IEphemeralSqliteConnection
{
    private readonly string path = Path.GetTempFileName();
    private bool disposed;

    private static SqliteConnection CreateConnection(string path)
    {
        return new SqliteConnection($"Data Source={path}");
    }

    /// <inheritdoc cref="IEphemeralSqliteConnection.CreateConnection" />
    public SqliteConnection CreateConnection() => CreateConnection(path);

    /// <inheritdoc />
    DbConnection IEphemeralSqliteConnection.CreateConnection() => CreateConnection();

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing">Whether this method is called by <see cref="Dispose()"/> or not.</param>
    protected virtual void Dispose(bool disposing)
    {
        try
        {
            if (disposed)
            {
                return;
            }

            File.Delete(path);
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
        await using var targetConn = CreateConnection(targetPath);
        await targetConn.OpenAsync(cancellationToken);
        await using var destinationConn = CreateConnection(destinationPath);
        targetConn.BackupDatabase(destinationConn);
    }

    /// <inheritdoc />
    public Task DumpDatabase(string destination, CancellationToken cancellationToken)
    {
        return BackupDatabase(path, destination, cancellationToken);
    }

    /// <inheritdoc />
    public Task LoadDatabase(string source, CancellationToken cancellationToken)
    {
        return BackupDatabase(source, path, cancellationToken);
    }

    /// <inheritdoc />
    ~EphemeralSqliteConnection()
    {
        Dispose(false);
    }
}
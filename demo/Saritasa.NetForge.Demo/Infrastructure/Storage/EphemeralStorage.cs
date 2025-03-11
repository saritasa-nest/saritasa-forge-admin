using Microsoft.Data.Sqlite;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public class EphemeralStorage : IEphemeralStorage
{
    private readonly HttpContext? httpContext;
    private readonly SqliteConnection connection;
    
    internal bool IsSeeded { get; set; }

    public EphemeralStorage(HttpContext? httpContext)
    {
        this.httpContext = httpContext;
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    }

    public SqliteConnection Connection => connection;

    public void Dispose()
    {
        connection.Dispose();
        if (httpContext != null)
        {
            EphemeralDatabaseRoot.Stores.TryRemove(httpContext, out _);
        }
    }
}
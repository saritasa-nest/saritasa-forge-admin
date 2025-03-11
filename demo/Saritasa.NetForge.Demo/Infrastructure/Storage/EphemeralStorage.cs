using Microsoft.Data.Sqlite;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public class EphemeralStorage : IEphemeralStorage
{
    private readonly SqliteConnection connection;

    public EphemeralStorage()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    }

    public SqliteConnection Connection => connection;

    public void Dispose()
    {
        connection.Dispose();
    }
}
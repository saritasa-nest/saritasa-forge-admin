using Microsoft.Data.Sqlite;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public interface IEphemeralStorage : IDisposable
{
    SqliteConnection Connection { get; }
}
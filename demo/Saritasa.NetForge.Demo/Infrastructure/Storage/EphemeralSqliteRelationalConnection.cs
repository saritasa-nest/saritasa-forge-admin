using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

/// <summary>
/// Ephemeral SQLite relational connection.
/// </summary>
internal class EphemeralSqliteRelationalConnection : RelationalConnection
{
    private readonly IEphemeralSqliteConnectionFactory ephemeralSqliteConnection;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public EphemeralSqliteRelationalConnection(RelationalConnectionDependencies dependencies,
        IEphemeralSqliteConnectionFactory ephemeralSqliteConnection)
        : base(dependencies)
    {
        this.ephemeralSqliteConnection = ephemeralSqliteConnection;
    }

    /// <inheritdoc />
    protected override DbConnection CreateDbConnection()
    {
        return ephemeralSqliteConnection.CreateConnection();
    }
}
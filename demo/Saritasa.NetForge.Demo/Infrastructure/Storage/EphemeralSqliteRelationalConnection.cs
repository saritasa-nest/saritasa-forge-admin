using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.Storage;

public class EphemeralSqliteRelationalConnection : RelationalConnection
{
    private readonly IEphemeralStorage ephemeralStorage;
    
    public EphemeralSqliteRelationalConnection(RelationalConnectionDependencies dependencies,
        IEphemeralStorage ephemeralStorage)
        : base(dependencies)
    {
        this.ephemeralStorage = ephemeralStorage;
    }

    protected override DbConnection CreateDbConnection()
    {
        return ephemeralStorage.Connection;
    }
}
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Saritasa.NetForge.Demo.Infrastructure.Seeders;
using Saritasa.NetForge.Demo.Infrastructure.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.Startup;

/// <summary>
/// Contains database migration helper methods.
/// </summary>
internal sealed class DatabaseInitializer : IAsyncInitializer
{
    private const int DefaultItemCount = 10;
    private readonly ShopDbContext dbContext;
    private readonly DbSnapshot dbSnapshot;
    private readonly IEnumerable<ISeeder> seeders;

    /// <summary>
    /// Database initializer. Performs migration and data seed.
    /// </summary>
    /// <param name="dbContext">Data context.</param>
    /// <param name="dbSnapshot">Database snapshot.</param>
    /// <param name="seeders">Seedeers.</param>
    public DatabaseInitializer(ShopDbContext dbContext, DbSnapshot dbSnapshot, IEnumerable<ISeeder> seeders)
    {
        this.dbContext = dbContext;
        this.dbSnapshot = dbSnapshot;
        this.seeders = seeders;
    }

    private async Task<string> DumpDatabase(CancellationToken cancellationToken)
    {
        // dbContext uses a service provider different from the one
        // our current scope uses.
        IInfrastructure<IServiceProvider> infrastructure = dbContext.Database;
        var ephemeralConnection = infrastructure.Instance.GetRequiredService<IEphemeralSqliteConnection>();
        var filename = Path.GetTempFileName();
        await ephemeralConnection.DumpDatabase(filename, cancellationToken);
        return filename;
    }

    /// <inheritdoc />
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
        foreach (var seeder in seeders)
        {
            await seeder.Seed(DefaultItemCount, cancellationToken);
        }
        var dumpLocation = await DumpDatabase(cancellationToken);
        dbSnapshot.SnapshotLocation = dumpLocation;
    }
}
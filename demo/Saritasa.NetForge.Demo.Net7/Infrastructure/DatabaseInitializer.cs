using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure;

/// <summary>
/// Contains database migration helper methods.
/// </summary>
internal sealed class DatabaseInitializer : IAsyncInitializer
{
    private readonly ShopDbContext dbContext;

    /// <summary>
    /// Database initializer. Performs migration and data seed.
    /// </summary>
    /// <param name="dbContext">Data context.</param>
    public DatabaseInitializer(ShopDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
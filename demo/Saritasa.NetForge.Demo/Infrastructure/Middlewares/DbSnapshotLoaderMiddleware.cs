using Microsoft.EntityFrameworkCore.Infrastructure;
using Saritasa.NetForge.Demo.Infrastructure.Extensions;
using Saritasa.NetForge.Demo.Infrastructure.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.Middlewares;

/// <summary>
/// Database snapshot loader middleware.
/// </summary>
internal sealed class DbSnapshotLoaderMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DbSnapshotLoaderMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Invokes the next middleware.
    /// </summary>
    /// <param name="httpContext">HTTP context.</param>
    public async Task Invoke(HttpContext httpContext)
    {
        var serviceProvider = httpContext.RequestServices;
        var snapshot = serviceProvider.GetRequiredService<DbSnapshot>();
        ArgumentNullException.ThrowIfNull(snapshot.SnapshotLocation);

        var cancellationToken = httpContext.RequestAborted;
        var dbContext = serviceProvider.GetRequiredService<ShopDbContext>();

        var ephemeralConnection = dbContext.Database
            .GetInstance()
            .GetRequiredService<IEphemeralSqliteConnectionFactory>();
        await ephemeralConnection.LoadDatabase(snapshot.SnapshotLocation, cancellationToken);
        await next(httpContext);
    }
}
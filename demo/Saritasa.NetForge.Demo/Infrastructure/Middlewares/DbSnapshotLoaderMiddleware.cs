using Microsoft.EntityFrameworkCore.Infrastructure;
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
        var sp = httpContext.RequestServices;
        var snapshot = sp.GetRequiredService<DbSnapshot>();
        ArgumentNullException.ThrowIfNull(snapshot.SnapshotLocation, nameof(snapshot.SnapshotLocation));

        var ct = httpContext.RequestAborted;
        var dbContext = sp.GetRequiredService<ShopDbContext>();
        IInfrastructure<IServiceProvider> infrastructure = dbContext.Database;

        var ephemeralConnection = infrastructure.Instance.GetRequiredService<IEphemeralSqliteConnection>();
        await ephemeralConnection.LoadDatabase(snapshot.SnapshotLocation, ct);
        await next(httpContext);
    }
}
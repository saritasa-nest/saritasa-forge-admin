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

        var dbContext = serviceProvider.GetRequiredService<ShopDbContext>();

        // We could not rely on HttpContext.RequestAborted,
        // probably related to this issue: https://github.com/dotnet/aspnetcore/issues/38917
        using var manualCancellationTokenSource = new CancellationTokenSource();
        httpContext.Items[SessionContext.HttpContextKey] = new SessionContext(manualCancellationTokenSource.Token);

        var ephemeralConnection = dbContext.Database
            .GetInstance()
            .GetRequiredService<IEphemeralSqliteConnectionFactory>();
        await ephemeralConnection.LoadDatabase(snapshot.SnapshotLocation, httpContext.RequestAborted);
        try
        {
            await next(httpContext);
        }
        finally
        {
            await manualCancellationTokenSource.CancelAsync();
        }
    }
}
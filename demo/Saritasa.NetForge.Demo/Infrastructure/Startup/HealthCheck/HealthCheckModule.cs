using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;
using Saritasa.NetForge.Demo.Infrastructure.Web;

namespace Saritasa.NetForge.Demo.Infrastructure.Startup.HealthCheck;

/// <summary>
/// Module responsible for configuring application health checks.
/// </summary>
internal static class HealthCheckModule
{
    /// <summary>
    /// Register health check endpoints.
    /// </summary>
    /// <param name="endpoints">Endpoints builder.</param>
    public static void Register(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/healthz", HealthCheckOptionsSetup.Setup(new HealthCheckOptions()));
        endpoints.MapGet("/readyz", context =>
        {
            var initializationStatus = context.RequestServices.GetRequiredService<AppInitializationStatusStorage>();

            context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            context.Response.StatusCode = initializationStatus.HasAppInitialized
                ? StatusCodes.Status200OK
                : StatusCodes.Status503ServiceUnavailable;
            return Task.CompletedTask;
        });
        endpoints.MapGet("/livez", context =>
        {
            context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Task.CompletedTask;
        });
    }
}

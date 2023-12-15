using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Net.Http.Headers;

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
        endpoints.MapHealthChecks("/health",
            new HealthCheckOptionsSetup().Setup(new HealthCheckOptions())
        );
        endpoints.MapGet("/liveness", context =>
        {
            context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Task.CompletedTask;
        });
    }
}

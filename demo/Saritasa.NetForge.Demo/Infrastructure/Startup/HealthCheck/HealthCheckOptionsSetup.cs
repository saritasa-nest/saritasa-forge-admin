using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Saritasa.NetForge.Demo.Infrastructure.Startup.HealthCheck;

/// <summary>
/// The class returns configured health check.
/// More health checks can be found here https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks .
/// </summary>
internal class HealthCheckOptionsSetup
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Returns <see cref="HealthCheckOptions" />.
    /// </summary>
    public HealthCheckOptions Setup(HealthCheckOptions options)
    {
        options.ResponseWriter = WriteResponseAsync;
        return options;
    }

    private static async Task WriteResponseAsync(HttpContext context, HealthReport report)
    {
        await context.Response.WriteAsJsonAsync(new
            {
                report.Status,
                Results = report.Entries.Select(e => new
                {
                    Name = e.Key,
                    e.Value.Status,
                    e.Value.Description
                })
            },
            JsonSerializerOptions);
    }
}
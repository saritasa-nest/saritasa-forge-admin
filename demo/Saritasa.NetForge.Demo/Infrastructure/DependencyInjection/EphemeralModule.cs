using Microsoft.Extensions.DependencyInjection.Extensions;
using Saritasa.NetForge.Demo.Infrastructure.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

public static class EphemeralModule
{
    public static void AddEphemeralSqlite(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<EphemeralStorage>();
        services.TryAddScoped<IEphemeralStorage>(static sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext != null)
            {
                sp = httpContext.RequestServices;
            }

            return sp.GetRequiredService<EphemeralStorage>();
        });
    }
}
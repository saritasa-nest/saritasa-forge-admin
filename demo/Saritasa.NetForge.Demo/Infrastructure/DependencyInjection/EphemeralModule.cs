using Microsoft.Extensions.DependencyInjection.Extensions;
using Saritasa.NetForge.Demo.Infrastructure.Storage;

namespace Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

public static class EphemeralModule
{
    public static void AddEphemeralSqlite(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<IEphemeralStorage>(static sp =>
        {
            var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return new EphemeralStorage(null);
            }

            return EphemeralDatabaseRoot.Stores
                .GetOrAdd(httpContext, static ctx => new EphemeralStorage(ctx));
        });
    }
}
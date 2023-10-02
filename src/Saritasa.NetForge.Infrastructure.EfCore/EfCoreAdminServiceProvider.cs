using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore.Services;

namespace Saritasa.NetForge.Infrastructure.EfCore;

/// <inheritdoc />
public class EfCoreAdminServiceProvider : IAdminOrmServiceProvider
{
    private readonly EfCoreOptionsBuilder efCoreOptionsBuilder;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EfCoreAdminServiceProvider(EfCoreOptionsBuilder efCoreOptionsBuilder)
    {
        this.efCoreOptionsBuilder = efCoreOptionsBuilder;
    }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        var efCoreOptions = efCoreOptionsBuilder.Create();
        services.TryAddSingleton(efCoreOptions);
        services.TryAddScoped<IOrmMetadataService, EfCoreMetadataService>();
    }
}

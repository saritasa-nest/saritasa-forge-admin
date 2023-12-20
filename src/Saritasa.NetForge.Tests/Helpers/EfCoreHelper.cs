using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;

namespace Saritasa.NetForge.Tests.Helpers;

/// <summary>
/// Ef core services helper.
/// </summary>
internal static class EfCoreHelper
{
    /// <summary>
    /// Creates <see cref="EfCoreDataService"/>.
    /// </summary>
    internal static EfCoreDataService CreateEfCoreDataService(DbContext dbContext)
    {
        var efCoreOptions = CreateEfCoreOptions(dbContext);
        var serviceProvider = CreateServiceProvider(dbContext);

        return new EfCoreDataService(efCoreOptions, serviceProvider);
    }

    /// <summary>
    /// Creates <see cref="EfCoreMetadataService"/>.
    /// </summary>
    internal static EfCoreMetadataService CreateEfCoreMetadataService(DbContext dbContext)
    {
        var efCoreOptions = CreateEfCoreOptions(dbContext);
        var serviceProvider = CreateServiceProvider(dbContext);

        return new EfCoreMetadataService(efCoreOptions, serviceProvider);
    }

    private static EfCoreOptions CreateEfCoreOptions(DbContext dbContext)
    {
        var efCoreOptions = new EfCoreOptions();
        efCoreOptions.DbContexts.Add(dbContext.GetType());

        return efCoreOptions;
    }

    private static IServiceProvider CreateServiceProvider(DbContext dbContext)
    {
        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider
            .Setup(provider => provider.GetService(dbContext.GetType()))
            .Returns(dbContext);

        return serviceProvider.Object;
    }
}

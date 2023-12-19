using Moq;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;

namespace Saritasa.NetForge.Tests.Utilities.Extensions;

/// <summary>
/// Extensions to <see cref="TestDbContext"/>. Useful for EF core related services.
/// </summary>
internal static class TestDbContextExtensions
{
    /// <summary>
    /// Creates <see cref="EfCoreDataService"/>.
    /// </summary>
    internal static EfCoreDataService CreateEfCoreDataService(
        this TestDbContext testDbContext, EfCoreOptions? efCoreOptions = null)
    {
        efCoreOptions ??= CreateEfCoreOptions();
        var serviceProvider = CreateServiceProvider(testDbContext);

        return new EfCoreDataService(efCoreOptions, serviceProvider);
    }

    /// <summary>
    /// Creates <see cref="EfCoreMetadataService"/>.
    /// </summary>
    internal static EfCoreMetadataService CreateEfCoreMetadataService(
        this TestDbContext testDbContext, EfCoreOptions? efCoreOptions = null)
    {
        efCoreOptions ??= CreateEfCoreOptions();
        var serviceProvider = CreateServiceProvider(testDbContext);

        return new EfCoreMetadataService(efCoreOptions, serviceProvider);
    }

    private static EfCoreOptions CreateEfCoreOptions()
    {
        var efCoreOptions = new EfCoreOptions();
        efCoreOptions.DbContexts.Add(typeof(TestDbContext));

        return efCoreOptions;
    }

    private static IServiceProvider CreateServiceProvider(TestDbContext testDbContext)
    {
        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider
            .Setup(provider => provider.GetService(typeof(TestDbContext)))
            .Returns(testDbContext);

        return serviceProvider.Object;
    }
}

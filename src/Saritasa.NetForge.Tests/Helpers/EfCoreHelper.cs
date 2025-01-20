using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;

namespace Saritasa.NetForge.Tests.Helpers;

/// <summary>
/// Ef core services helper.
/// </summary>
internal static class EfCoreHelper
{
    /// <summary>
    /// Creates <see cref="TestDbContext"/>.
    /// </summary>
    internal static TestDbContext CreateTestDbContext()
    {
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var testDbContext = new TestDbContext(dbOptions);
        testDbContext.Database.EnsureCreated();

        return testDbContext;
    }

    /// <summary>
    /// Creates <see cref="EfCoreDataService"/>.
    /// </summary>
    internal static EfCoreDataService CreateEfCoreDataService(DbContext dbContext)
    {
        var efCoreOptions = CreateEfCoreOptions(dbContext);
        var serviceProvider = CreateServiceProvider(dbContext);

        var adminOptionsBuilder = new AdminOptionsBuilder();
        var adminMetadataService = new AdminMetadataService(
            CreateEfCoreMetadataService(dbContext),
            adminOptionsBuilder.Create(),
            MemoryCacheHelper.CreateMemoryCache());
        var entityService = new EntityService(adminMetadataService, serviceProvider);

        return new EfCoreDataService(efCoreOptions, serviceProvider, entityService);
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

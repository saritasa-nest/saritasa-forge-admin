using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;
using Xunit;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Create entity tests.
/// </summary>
public class EntitySearchTests : IDisposable
{
    private TestDbContext DbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntitySearchTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("NetForgeTest")
            .Options;

        DbContext = new TestDbContext(dbOptions);
        DbContext.Database.EnsureCreated();

        DbContext.Addresses.Add(new Address
        {
            Id = 1,
            Street = "Main St."
        });
        DbContext.Addresses.Add(new Address
        {
            Id = 2,
            Street = "Main Square St."
        });
        DbContext.Addresses.Add(new Address
        {
            Id = 3,
            Street = "Second Square St."
        });
        DbContext.Addresses.Add(new Address
        {
            Id = 4,
            Street = "Second main St."
        });
        DbContext.Addresses.Add(new Address
        {
            Id = 5,
            Street = "Central"
        });
        DbContext.SaveChanges();
    }

    private bool disposedValue;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Deletes the database after one test is complete,
    /// so it gives us the same state of the database for every test.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DbContext.Database.EnsureDeleted();
                DbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    private static EfCoreDataService CreateEfCoreDataService(TestDbContext testDbContext)
    {
        var efCoreOptions = new EfCoreOptions();
        var shopDbContextType = typeof(TestDbContext);
        efCoreOptions.DbContexts.Add(shopDbContextType);

        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider
            .Setup(provider => provider.GetService(shopDbContextType))
            .Returns(testDbContext);

        return new EfCoreDataService(efCoreOptions, serviceProvider.Object);
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_ValidSearch_ShouldFind3()
    {
        // Arrange
        var automapper = new Mock<IMapper>();

        var ormMetadataService = new Mock<IOrmMetadataService>();
        var adminOptions = new AdminOptions();
        var memoryCache = new Mock<IMemoryCache>();
        var adminMetadataService = new AdminMetadataService(ormMetadataService.Object, adminOptions, memoryCache.Object);

        var efCoreDataService = CreateEfCoreDataService(DbContext);
        var serviceProvider = new Mock<IServiceProvider>();

        var entityService = new EntityService(
            automapper.Object, adminMetadataService, efCoreDataService, serviceProvider.Object);

        var entityType = typeof(Address);

        var searchOptions = new SearchOptions
        {
            SearchString = "ain"
        };

        const int expectedDataCount = 3;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(entityType, new List<PropertyMetadataDto>(), searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }
}

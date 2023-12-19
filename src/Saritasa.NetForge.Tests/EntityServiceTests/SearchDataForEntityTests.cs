using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Utilities.Extensions;
using Saritasa.NetForge.Tests.Utilities.Helpers;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;
using Xunit;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Create entity tests.
/// </summary>
public class SearchDataForEntityTests : IDisposable
{
    private TestDbContext TestDbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchDataForEntityTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("NetForgeTest")
            .Options;

        TestDbContext = new TestDbContext(dbOptions);
        TestDbContext.Database.EnsureCreated();

        TestDbContext.Addresses.Add(new Address
        {
            Id = 1,
            Street = "Main St."
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 2,
            Street = "Main Square St."
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 3,
            Street = "Second Square St."
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 4,
            Street = "Second main St."
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 5,
            Street = "Central"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 6,
            Street = "central street"
        });
        TestDbContext.SaveChanges();
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
                TestDbContext.Database.EnsureDeleted();
                TestDbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    private static AdminOptions CreateAdminOptionsWithSearchType(SearchType searchType)
    {
        var adminOptionsBuilder = new AdminOptionsBuilder();
        adminOptionsBuilder.ConfigureEntity<Address>(entityOptionsBuilder =>
        {
            entityOptionsBuilder.ConfigureProperty(address => address.Street, propertyBuilder =>
            {
                propertyBuilder.SetSearchType(searchType);
            });
        });
        return adminOptionsBuilder.Create();
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ContainsCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_ContainsCaseInsensitiveSearch_ShouldFind3()
    {
        // Arrange
        var automapper = AutomapperHelper.CreateAutomapper();

        var efCoreMetadataService = TestDbContext.CreateEfCoreMetadataService();
        var adminOptions = CreateAdminOptionsWithSearchType(SearchType.ContainsCaseInsensitive);
        var memoryCache = MemoryCacheHelper.CreateMemoryCache();
        var adminMetadataService =
            new AdminMetadataService(efCoreMetadataService, adminOptions, memoryCache);

        var efCoreDataService = TestDbContext.CreateEfCoreDataService();

        var serviceProvider = new Mock<IServiceProvider>();

        var entityService =
            new EntityService(automapper, adminMetadataService, efCoreDataService, serviceProvider.Object);

        var searchOptions = new SearchOptions
        {
            SearchString = "ain"
        };

        const string addressEntityName = "Addresses";
        var addressEntity = await entityService.GetEntityByIdAsync(addressEntityName, CancellationToken.None);

        const int expectedDataCount = 3;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.StartsWithCaseSensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_StartsWithCaseSensitiveSearch_ShouldFind2()
    {
        // Arrange
        var automapper = AutomapperHelper.CreateAutomapper();

        var efCoreMetadataService = TestDbContext.CreateEfCoreMetadataService();
        var adminOptions = CreateAdminOptionsWithSearchType(SearchType.StartsWithCaseSensitive);
        var memoryCache = MemoryCacheHelper.CreateMemoryCache();
        var adminMetadataService =
            new AdminMetadataService(efCoreMetadataService, adminOptions, memoryCache);

        var efCoreDataService = TestDbContext.CreateEfCoreDataService();

        var serviceProvider = new Mock<IServiceProvider>();

        var entityService =
            new EntityService(automapper, adminMetadataService, efCoreDataService, serviceProvider.Object);

        var searchOptions = new SearchOptions
        {
            SearchString = "Second"
        };

        const string addressEntityName = "Addresses";
        var addressEntity = await entityService.GetEntityByIdAsync(addressEntityName, CancellationToken.None);

        const int expectedDataCount = 2;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_ExactMatchCaseInsensitiveSearch_ShouldFind1()
    {
        // Arrange
        var automapper = AutomapperHelper.CreateAutomapper();

        var efCoreMetadataService = TestDbContext.CreateEfCoreMetadataService();
        var adminOptions = CreateAdminOptionsWithSearchType(SearchType.ExactMatchCaseInsensitive);
        var memoryCache = MemoryCacheHelper.CreateMemoryCache();
        var adminMetadataService =
            new AdminMetadataService(efCoreMetadataService, adminOptions, memoryCache);

        var efCoreDataService = TestDbContext.CreateEfCoreDataService();

        var serviceProvider = new Mock<IServiceProvider>();

        var entityService =
            new EntityService(automapper, adminMetadataService, efCoreDataService, serviceProvider.Object);

        var searchOptions = new SearchOptions
        {
            SearchString = "Central"
        };

        const string addressEntityName = "Addresses";
        var addressEntity = await entityService.GetEntityByIdAsync(addressEntityName, CancellationToken.None);

        const int expectedDataCount = 1;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }
}

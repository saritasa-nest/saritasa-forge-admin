using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Utilities.Extensions;
using Saritasa.NetForge.Tests.Utilities.Helpers;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
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
            Street = "Main St.",
            City = "New York"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 2,
            Street = "Main Square St.",
            City = "London"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 3,
            Street = "Second Square St.",
            City = "London"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 4,
            Street = "Second main St.",
            City = "New York"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 5,
            Street = "Central",
            City = "London"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 6,
            Street = "central street",
            City = "New York"
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 7
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 8
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 9
        });
        TestDbContext.Addresses.Add(new Address
        {
            Id = 10
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

    private EntityService entityService = null!;

    private SearchOptions searchOptions = null!;

    private GetEntityByIdDto addressEntity = null!;

    private EfCoreDataService efCoreDataService = null!;

    private async Task InitializeTestServicesAsync(SearchType searchType, string searchString)
    {
        var automapper = AutomapperHelper.CreateAutomapper();

        var efCoreMetadataService = TestDbContext.CreateEfCoreMetadataService();
        var adminOptions = CreateAdminOptionsWithSearchType(searchType);
        var memoryCache = MemoryCacheHelper.CreateMemoryCache();
        var adminMetadataService =
            new AdminMetadataService(efCoreMetadataService, adminOptions, memoryCache);

        efCoreDataService = TestDbContext.CreateEfCoreDataService();

        var serviceProvider = new Mock<IServiceProvider>();

        entityService =
            new EntityService(automapper, adminMetadataService, efCoreDataService, serviceProvider.Object);

        searchOptions = new SearchOptions
        {
            SearchString = searchString
        };

        const string addressEntityName = "Addresses";
        addressEntity = await entityService.GetEntityByIdAsync(addressEntityName, CancellationToken.None);
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

            entityOptionsBuilder.ConfigureProperty(address => address.City, propertyBuilder =>
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
    public async Task Search_ContainsCaseInsensitive_ShouldFind3()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.ContainsCaseInsensitive, "ain");

        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive)
        };

        const int expectedDataCount = 3;

        // Act
        var searchedData =
            efCoreDataService.Search(
                TestDbContext.Addresses, searchOptions.SearchString, addressEntity.ClrType, propertiesWithSearchTypes);

        // Assert

        var actualDataCount = await searchedData.CountAsync();
        Assert.Equal(expectedDataCount, actualDataCount);
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ContainsCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_ContainsCaseInsensitiveSearch_ShouldFind3()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.ContainsCaseInsensitive, "ain");
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
        await InitializeTestServicesAsync(SearchType.StartsWithCaseSensitive, "Second");
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
        await InitializeTestServicesAsync(SearchType.ExactMatchCaseInsensitive, "Central");
        const int expectedDataCount = 1;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/> to search values that contain <see langword="null"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_ExactMatchCaseInsensitiveNullSearch_ShouldFind4()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.ExactMatchCaseInsensitive, "None");
        const int expectedDataCount = 4;

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
    public async Task SearchDataForEntityAsync_WithoutSearch_ShouldFindAll()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.None, string.Empty);
        var expectedDataCount = await TestDbContext.Addresses.CountAsync();

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }

    /// <summary>
    /// Test for <seealso cref="EntityService.SearchDataForEntityAsync"/>
    /// when <see cref="SearchOptions.SearchString"/> contains multiple words.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_MultipleSearchWords_ShouldFind2()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.ContainsCaseInsensitive, "sq lond");
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
    /// when <see cref="SearchOptions.SearchString"/> contains phrase with quotes.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntityAsync_SearchStringWithQuotes_ShouldFind2()
    {
        // Arrange
        await InitializeTestServicesAsync(SearchType.ContainsCaseInsensitive, "\"main St.\"");
        const int expectedDataCount = 2;

        // Act
        var searchedData =
            await entityService.SearchDataForEntityAsync(addressEntity.ClrType, addressEntity.Properties, searchOptions,
                searchFunction: null, customQueryFunction: null);

        // Assert
        Assert.Equal(expectedDataCount, searchedData.Metadata.TotalCount);
    }
}

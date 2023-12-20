using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
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
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        TestDbContext = new TestDbContext(dbOptions);
        TestDbContext.Database.EnsureCreated();

        TestDbContext.Addresses.Add(new Address
        {
            Street = "Main St.",
            City = "New York",
            Latitude = 100
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Main Square St.",
            City = "London",
            Latitude = 101
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Second Square St.",
            City = "London",
            Latitude = 102
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Second main St.",
            City = "New York",
            Latitude = 10
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Central",
            City = "London",
            Latitude = 222
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "central street",
            City = "New York",
            Latitude = 1
        });

        TestDbContext.Products.Add(new Product
        {
            Supplier = new Supplier()
        });
        TestDbContext.Products.Add(new Product
        {
            WeightInGrams = 111,
            Supplier = new Supplier
            {
                Name = "Supplier",
                City = "London"
            }
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

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/>
    /// using <see cref="SearchType.ContainsCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task Search_ContainsCaseInsensitive_ShouldFind3()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "ain";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 3;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/>
    /// using <see cref="SearchType.StartsWithCaseSensitive"/>.
    /// </summary>
    [Fact]
    public async Task Search_StartsWithCaseSensitive_ShouldFind2()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "Second";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.StartsWithCaseSensitive)
        };

        const int expectedCount = 2;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task Search_ExactMatchCaseInsensitive_ShouldFind1()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "Central";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ExactMatchCaseInsensitive)
        };

        const int expectedCount = 1;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/> to search values that contain <see langword="null"/>.
    /// </summary>
    [Fact]
    public async Task Search_ExactMatchCaseInsensitive_WithNoneSearchString_ShouldFind1()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "None";
        var entityType = typeof(Product);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Product.WeightInGrams), SearchType.ExactMatchCaseInsensitive)
        };

        const int expectedCount = 1;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Products, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/> when <see cref="SearchType.None"/>.
    /// </summary>
    [Fact]
    public async Task Search_WithoutSearch_ShouldFindAll()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        var searchString = "SearchString";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.None)
        };

        var expectedCount = await TestDbContext.Addresses.CountAsync();

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/> when search string contains multiple words.
    /// </summary>
    [Fact]
    public async Task Search_WithMultipleWords_ShouldFind2()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "sq lond";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive),
            (nameof(Address.City), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 2;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert

        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/> when search string contains quoted phrase.
    /// </summary>
    [Fact]
    public async Task Search_WithQuotedPhrase_ShouldFind2()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "\"main St.\"";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 2;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert
        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/> when searched property is not <see cref="string"/>.
    /// </summary>
    [Fact]
    public async Task Search_NotStringType_ShouldFind4()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

        const string searchString = "10";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Latitude), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 4;

        // Act
        var searchedData =
            efCoreDataService.Search(TestDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert
        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }
}

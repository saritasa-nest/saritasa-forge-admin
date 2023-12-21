using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Create entity tests.
/// </summary>
public class SearchTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDbContext testDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchTests(TestDatabaseFixture testDatabaseFixture)
    {
        testDbContext = testDatabaseFixture.TestDbContext;
    }

    /// <summary>
    /// Test for <seealso cref="EfCoreDataService.Search"/>
    /// using <see cref="SearchType.ContainsCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task Search_ContainsCaseInsensitive_ShouldFind3()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "ain";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 3;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "Second";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.StartsWithCaseSensitive)
        };

        const int expectedCount = 2;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "Central";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ExactMatchCaseInsensitive)
        };

        const int expectedCount = 1;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "None";
        var entityType = typeof(Product);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Product.WeightInGrams), SearchType.ExactMatchCaseInsensitive)
        };

        const int expectedCount = 1;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Products, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        var searchString = "SearchString";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.None)
        };

        var expectedCount = await testDbContext.Addresses.CountAsync();

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

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
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "\"main St.\"";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Street), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 2;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        const string searchString = "10";
        var entityType = typeof(Address);
        var propertiesWithSearchTypes = new List<(string, SearchType)>
        {
            (nameof(Address.Latitude), SearchType.ContainsCaseInsensitive)
        };

        const int expectedCount = 4;

        // Act
        var searchedData =
            efCoreDataService.Search(testDbContext.Addresses, searchString, entityType, propertiesWithSearchTypes);

        // Assert
        var actualCount = await searchedData.CountAsync();
        Assert.Equal(expectedCount, actualCount);
    }
}

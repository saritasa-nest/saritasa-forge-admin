using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Domain.UseCases.Common;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests.SearchDataForEntityTests;

/// <summary>
/// Searching tests.
/// </summary>
public class SearchTests : IClassFixture<SearchTestsFixture>
{
    private readonly TestDbContext testDbContext;
    private readonly IOrmDataService dataService;

    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchTests(SearchTestsFixture fixture)
    {
        testDbContext = fixture.GetService<TestDbContext>();
        dataService = fixture.GetService<IOrmDataService>();
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ContainsCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_ContainsCaseInsensitive_ShouldFind3()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Street), SearchType = SearchType.ContainsCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "ain" };

        const int expectedCount = 3;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_ExactMatchCaseInsensitive_ShouldFind1()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Street), SearchType = SearchType.ExactMatchCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "Central" };

        const int expectedCount = 1;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/>
    /// using <see cref="SearchType.ExactMatchCaseInsensitive"/> to search values that contain <see langword="null"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_ExactMatchCaseInsensitive_WithNoneSearchString_ShouldFind1()
    {
        // Arrange
        var entityType = typeof(Product);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Product.WeightInGrams), SearchType = SearchType.ExactMatchCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "None" };

        const int expectedCount = 1;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/> when <see cref="SearchType.None"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_WithoutSearch_ShouldFindAll()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Street), SearchType = SearchType.None }
        };
        var searchOptions = new SearchOptions { SearchString = "SearchString" };

        var expectedCount = await testDbContext.Addresses.CountAsync(cancellationToken);

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/> when search string contains multiple words.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_WithMultipleWords_ShouldFind2()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Street), SearchType = SearchType.ContainsCaseInsensitive },
            new() { Name = nameof(Address.City), SearchType = SearchType.ContainsCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "sq lond" };

        const int expectedCount = 2;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/> when search string contains quoted phrase.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_WithQuotedPhrase_ShouldFind2()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Street), SearchType = SearchType.ContainsCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "\"main St.\"" };

        const int expectedCount = 2;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/> when searched property is not <see cref="string"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_NotStringType_ShouldFind4()
    {
        // Arrange
        var entityType = typeof(Address);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Address.Latitude), SearchType = SearchType.ContainsCaseInsensitive }
        };
        var searchOptions = new SearchOptions { SearchString = "10" };

        const int expectedCount = 4;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Test for <seealso cref="IOrmDataService.SearchDataForEntityAsync"/> when searched property is not <see cref="string"/>.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_ByPropertyInsideNavigation_ShouldFind1()
    {
        // Arrange
        var entityType = typeof(Product);
        var properties = new List<PropertyMetadataDto>
        {
            new NavigationMetadataDto
            {
                Name = nameof(Product.Supplier),
                ClrType = typeof(Supplier),
                TargetEntityProperties = [new PropertyMetadataDto
                {
                    Name = nameof(Supplier.City),
                    SearchType = SearchType.ContainsCaseInsensitive
                }
                ]
            }
        };
        var searchOptions = new SearchOptions { SearchString = "London" };

        const int expectedCount = 1;

        // Act
        var searchedData = await dataService.SearchDataForEntityAsync(entityType, properties, searchOptions);

        // Assert
        var actualCount = searchedData.Metadata.TotalCount;
        Assert.Equal(expectedCount, actualCount);
    }
}

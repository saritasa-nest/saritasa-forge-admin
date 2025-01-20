using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Fixtures;
using Saritasa.NetForge.Tests.Helpers;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Searching tests.
/// </summary>
[CollectionDefinition(Constants.DependencyInjection)]
public class SearchTests : TestBed<NetForgeFixture>
{
#pragma warning disable CA2213
    private readonly TestDbContext testDbContext;
#pragma warning restore CA2213
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchTests(ITestOutputHelper testOutputHelper, NetForgeFixture netForgeFixture)
        : base(testOutputHelper, netForgeFixture)
    {
        testDbContext = netForgeFixture.GetService<TestDbContext>(testOutputHelper)!;
        dataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);

        PopulateDatabaseWithTestData();

        var services = new ServiceCollection();
        services.AddScoped<TestDbContext>();
    }

    private void PopulateDatabaseWithTestData()
    {
        if (testDbContext.Addresses.Any())
        {
            return;
        }

        testDbContext.Addresses.Add(new Address
        {
            Street = "Main St.",
            City = "New York",
            Latitude = 100
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Main Square St.",
            City = "London",
            Latitude = 101
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Second Square St.",
            City = "London",
            Latitude = 102
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Second main St.",
            City = "New York",
            Latitude = 10
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Central",
            City = "London",
            Latitude = 222
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "central street",
            City = "New York",
            Latitude = 1
        });

        testDbContext.Products.Add(new Product
        {
            Supplier = new Supplier()
        });
        testDbContext.Products.Add(new Product
        {
            WeightInGrams = 111,
            Supplier = new Supplier
            {
                Name = "Supplier",
                City = "London"
            }
        });
        testDbContext.Products.Add(new Product
        {
            WeightInGrams = 222,
            Supplier = new Supplier
            {
                Name = "ABC",
                City = "New-York"
            }
        });
        testDbContext.SaveChanges();
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

        var expectedCount = await testDbContext.Addresses.CountAsync();

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

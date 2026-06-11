using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.UseCases.Common;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests.SearchDataForEntityTests;

/// <summary>
/// Tests for <see cref="IOrmDataService.SearchDataForEntityAsync"/> using a custom query function.
/// </summary>
public class CustomQueryTests : IClassFixture<CustomQueryTestsFixture>
{
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CustomQueryTests(CustomQueryTestsFixture fixture)
    {
        dataService = fixture.GetService<IOrmDataService>();
    }

    /// <summary>
    /// When <see cref="EntityOptionsBuilder{TEntity}.ConfigureCustomQuery"/>
    /// uses <c>Where</c> with a plain property (no navigation),
    /// the query should be translatable and return only the matching entities.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_CustomQuery_ShouldReturnOnlyMatchingEntities()
    {
        // Arrange
        var entityType = typeof(Shop);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Shop.Name) }
        };
        var searchOptions = new SearchOptions();

        Func<IServiceProvider?, IQueryable<object>, IQueryable<object>> customQueryFunction =
            (_, query) => query.Cast<Shop>().Where(shop => shop.Name == "London Shop");

        const int expectedCount = 1;

        // Act
        var result = await dataService.SearchDataForEntityAsync(
            entityType,
            properties,
            searchOptions,
            customQueryFunction: customQueryFunction);

        // Assert
        Assert.Equal(expectedCount, result.Metadata.TotalCount);
    }

    /// <summary>
    /// When <see cref="EntityOptionsBuilder{TEntity}.ConfigureCustomQuery"/>
    /// uses <c>Where</c> with a collection navigation property,
    /// the query should be translatable and return only the matching entities.
    /// </summary>
    [Fact]
    public async Task SearchDataForEntity_CustomQueryWithNavigationFilter_ShouldReturnOnlyMatchingEntities()
    {
        // Arrange
        var entityType = typeof(Shop);
        var properties = new List<PropertyMetadataDto>
        {
            new() { Name = nameof(Shop.Name) }
        };
        var searchOptions = new SearchOptions();

        Func<IServiceProvider?, IQueryable<object>, IQueryable<object>> customQueryFunction =
            (_, query) => query.Cast<Shop>().Where(shop => shop.Suppliers.Any(supplier => supplier.City == "London"));

        const int expectedCount = 1;

        // Act
        var result = await dataService.SearchDataForEntityAsync(
            entityType,
            properties,
            searchOptions,
            customQueryFunction: customQueryFunction);

        // Assert
        Assert.Equal(expectedCount, result.Metadata.TotalCount);
    }
}

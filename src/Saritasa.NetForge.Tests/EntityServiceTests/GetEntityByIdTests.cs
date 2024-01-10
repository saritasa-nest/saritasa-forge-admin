using System.ComponentModel;
using Moq;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Saritasa.NetForge.Tests.Utilities;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;
using Saritasa.Tools.Domain.Exceptions;
using Xunit;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Tests for <see cref="EntityService.GetEntityByIdAsync"/>.
/// </summary>
public class GetEntityByIdTests : IDisposable
{
    private const string ContactInfoId = "ContactInfoes";
    private const string ShopId = "Shops";

    private readonly TestDbContext testDbContext;
    private readonly IEntityService entityService;
    private readonly AdminOptionsBuilder adminOptionsBuilder;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
        adminOptionsBuilder = new AdminOptionsBuilder();
        var adminMetadataService = new AdminMetadataService(
            EfCoreHelper.CreateEfCoreMetadataService(testDbContext),
            adminOptionsBuilder.Create(),
            MemoryCacheHelper.CreateMemoryCache());

        entityService = new EntityService(
            AutomapperHelper.CreateAutomapper(),
            adminMetadataService,
            EfCoreHelper.CreateEfCoreDataService(testDbContext),
            new Mock<IServiceProvider>().Object);
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
                testDbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Test for case when string id is valid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_ValidStringId_ShouldBeNotNull()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(ContactInfoId, CancellationToken.None);

        // Assert
        Assert.NotNull(entity);
    }

    /// <summary>
    /// Test for case when string id is invalid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_InvalidStringId_ShouldThrowNotFoundException()
    {
        // Arrange
        const string invalidStringId = "ContactInfoes2";

        // Act
        var getEntityByIdCall = () => entityService.GetEntityByIdAsync(invalidStringId, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(getEntityByIdCall);
    }

    /// <summary>
    /// Test for case when navigation included to entity.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithNavigations_ShouldBeNotNull()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.IncludeNavigations(entity => entity.Address);
        });

        const string navigationPropertyName = nameof(Shop.Address);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.Name.Equals(navigationPropertyName));
    }

    /// <summary>
    /// Test for case when property excluded from query via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithExcludedFromQueryPropertyViaFluentApi_ShouldNotContainExcludedProperty()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder
                .ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsExcludedFromQuery(true));
        });

        const string excludedPropertyName = nameof(Shop.IsOpen);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(entity.Properties, property => property.Name.Equals(excludedPropertyName));
    }

    /// <summary>
    /// Test for case when property excluded from query via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithExcludedFromQueryPropertyViaAttribute_ShouldNotContainExcludedProperty()
    {
        // Arrange
        const string excludedPropertyName = nameof(Shop.IsOpen);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(entity.Properties, property => property.Name.Equals(excludedPropertyName));
    }

    /// <summary>
    /// Test for case when properties don't have ordering.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithoutOrdering_PrimaryKeyShouldBeFirst()
    {
        // Arrange
        const string expectedPropertyName = nameof(ContactInfo.Id);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ContactInfoId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First();
        Assert.Equal(expectedPropertyName, firstProperty.Name);
    }

    /// <summary>
    /// Test for case when properties have ordering via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithOrderingViaFluentApi_OrderedPropertyShouldBeFirst()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder
                .ConfigureProperty(shop => shop.TotalSales, optionsBuilder => optionsBuilder.SetOrder(0))
                .ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetOrder(1))
                .ConfigureProperty(shop => shop.Name, optionsBuilder => optionsBuilder.SetOrder(2))
                .ConfigureProperty(shop => shop.OpenedDate, optionsBuilder => optionsBuilder.SetOrder(3))
                .ConfigureProperty(shop => shop.Id, optionsBuilder => optionsBuilder.SetOrder(4));
        });
        const string expectedPropertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First();
        Assert.Equal(expectedPropertyName, firstProperty.Name);
    }

    /// <summary>
    /// Test for case when properties have ordering via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithOrderingViaAttribute_OrderedPropertyShouldBeFirst()
    {
        // Arrange
        const string expectedPropertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First();
        Assert.Equal(expectedPropertyName, firstProperty.Name);
    }

    /// <summary>
    /// Test for case when property has set display name via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaFluentApi_DisplayNameShouldChange()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.TotalSales,
                    optionsBuilder => optionsBuilder.SetDisplayName(ShopConstants.TotalSalesDisplayName));
        });
        const string propertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.TotalSalesDisplayName, firstProperty.DisplayName);
    }

    /// <summary>
    /// Test for case when property has set display name via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaAttribute_DisplayNameShouldChange()
    {
        // Arrange
        const string propertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.TotalSalesDisplayName, firstProperty.DisplayName);
    }

    /// <summary>
    /// Test for case when property has set display name via <see cref="DisplayNameAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDisplayNameViaBuiltInAttribute_DisplayNameShouldChange()
    {
        // Arrange
        const string propertyName = nameof(Shop.Name);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.NameDisplayName, firstProperty.DisplayName);
    }

    /// <summary>
    /// Test for case when property has set description via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaFluentApi_DescriptionShouldChange()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(
                shop => shop.TotalSales,
                optionsBuilder => optionsBuilder.SetDescription(ShopConstants.TotalSalesDescription));
        });
        const string propertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.TotalSalesDescription, firstProperty.Description);
    }

    /// <summary>
    /// Test for case when property has set description via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaAttribute_DescriptionShouldChange()
    {
        // Arrange
        const string propertyName = nameof(Shop.TotalSales);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.TotalSalesDescription, firstProperty.Description);
    }

    /// <summary>
    /// Test for case when property has set description via <see cref="DescriptionAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithPropertyDescriptionViaBuiltInAttribute_DescriptionShouldChange()
    {
        // Arrange
        const string propertyName = nameof(Shop.Name);

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        var firstProperty = entity.Properties.First(property => property.Name.Equals(propertyName));
        Assert.Equal(ShopConstants.NameDescription, firstProperty.Description);
    }

    /// <summary>
    /// Test for case when property is hidden via Fluent API.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenPropertyViaFluentApi_PropertyShouldBeHidden()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsHidden(true));
        });

        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHidden);
    }

    /// <summary>
    /// Test for case when property is hidden via <see cref="NetForgePropertyAttribute"/>.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithHiddenPropertyViaAttribute_PropertyShouldBeHidden()
    {
        // Act
        var entity = await entityService.GetEntityByIdAsync(ShopId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.IsHidden);
    }
}

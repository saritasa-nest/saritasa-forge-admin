using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Fixtures;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Tests for <see cref="IOrmDataService.UpdateAsync"/>.
/// </summary>
public class UpdateEntityTests : TestBed<NetForgeFixture>
{
#pragma warning disable CA2213
    private readonly TestDbContext testDbContext;
#pragma warning restore CA2213
    private readonly IOrmDataService efCoreDataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UpdateEntityTests(ITestOutputHelper testOutputHelper, NetForgeFixture netForgeFixture)
        : base(testOutputHelper, netForgeFixture)
    {
        testDbContext = _fixture.GetService<TestDbContext>(_testOutputHelper)!;
        efCoreDataService = _fixture.GetService<IOrmDataService>(_testOutputHelper)!;

        var shops = Fakers.ShopFaker.Generate(2);
        testDbContext.Shops.AddRange(shops);
        testDbContext.SaveChanges();

        testDbContext.ChangeTracker.Clear();
    }

    /// <summary>
    /// Update valid entity.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_ValidEntity_Success()
    {
        // Arrange
        var updatedShop = await testDbContext.Shops.AsNoTracking().FirstAsync();
        var originalShop = updatedShop.CloneJson()!;

        const string newName = "Test222";
        updatedShop.Name = newName;

        // Act
        await efCoreDataService.UpdateAsync(updatedShop, originalShop, CancellationToken.None);

        // Assert
        Assert.Contains(testDbContext.Shops, shop => shop.Name.Equals(newName));
    }

    /// <summary>
    /// Update entity navigation reference.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_NavigationReference_Success()
    {
        // Arrange
        var shops = testDbContext.Shops.Include(shop => shop.Address).AsNoTracking();

        var updatedShop = await shops.FirstAsync();
        var originalShop = updatedShop.CloneJson()!;

        var newAddress = Fakers.AddressFaker.Generate();
        updatedShop.Address = newAddress;

        // Act
        await efCoreDataService.UpdateAsync(updatedShop, originalShop, CancellationToken.None);

        // Assert
        Assert.Contains(shops, shop => shop.Address!.Street.Equals(newAddress.Street));
    }

    /// <summary>
    /// Update entity navigation reference to null value.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_NavigationReferenceToNull_Success()
    {
        // Arrange
        var shops = testDbContext.Shops.Include(shop => shop.Address).AsNoTracking();

        var updatedShop = await shops.FirstAsync();
        var originalShop = updatedShop.CloneJson()!;

        updatedShop.Address = null;

        // Act
        await efCoreDataService.UpdateAsync(updatedShop, originalShop, CancellationToken.None);

        // Assert
        Assert.Contains(shops, shop => shop.Address is null);
    }

    /// <summary>
    /// Update entity with adding new element to navigation collection.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_AddNewElementToNavigationCollection_ElementShouldBeCreatedAndAdded()
    {
        // Arrange
        var shops = testDbContext.Shops.Include(shop => shop.Products).AsNoTracking();

        var updatedShop = await shops.FirstAsync();
        var originalShop = updatedShop.CloneJson()!;

        var newProduct = Fakers.ProductFaker.Generate();
        updatedShop.Products.Add(newProduct);

        // Act
        await efCoreDataService.UpdateAsync(updatedShop, originalShop, CancellationToken.None);

        // Assert
        Assert.Contains(testDbContext.Products, product => product.Id == newProduct.Id);
    }
}

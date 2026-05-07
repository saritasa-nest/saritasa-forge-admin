using DeepCopy;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Tests for entities with strongly-typed IDs.
/// </summary>
public class StronglyTypedIdTests : IDisposable
{
    private readonly TestDbContext testDbContext;
    private readonly IOrmDataService efCoreDataService;

    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;

    /// <summary>
    /// Constructor.
    /// </summary>
    public StronglyTypedIdTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
        efCoreDataService = EfCoreHelper.CreateEfCoreDataService(testDbContext);
    }

    private bool disposedValue;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Deletes the database after each test to ensure a clean state.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            testDbContext.Dispose();
        }

        disposedValue = true;
    }

    /// <summary>
    /// Verifies that an entity with a strongly-typed primary key can be added to the database.
    /// </summary>
    [Fact]
    public async Task CreateEntity_StronglyTypedPrimaryKey_Success()
    {
        // Arrange
        var token = Fakers.TokenFaker.Generate();

        // Act
        await efCoreDataService.AddAsync(token, typeof(Token), cancellationToken);

        // Assert
        Assert.Contains(testDbContext.Tokens, t => t.Id.Equals(token.Id));
    }

    /// <summary>
    /// Verifies that an entity whose FK is a strongly-typed ID can be updated.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_StronglyTypedForeignKey_ShouldUpdate()
    {
        // Arrange
        var token = Fakers.TokenFaker.Generate();
        testDbContext.Tokens.Add(token);
        var newToken = Fakers.TokenFaker.Generate();
        testDbContext.Tokens.Add(newToken);

        var shop = Fakers.ShopFaker.Generate();
        shop.Token = token;
        shop.TokenId = token.Id;
        testDbContext.Shops.Add(shop);

        await testDbContext.SaveChangesAsync(cancellationToken);
        testDbContext.ChangeTracker.Clear();

        var updatedShop = await testDbContext.Shops.AsNoTracking().FirstAsync(s => s.Id == shop.Id, cancellationToken);
        var originalShop = ObjectCloner.Clone(updatedShop)!;

        updatedShop.Token = newToken;

        // Act
        await efCoreDataService.UpdateAsync(updatedShop, originalShop, afterUpdateAction: null, cancellationToken);

        // Assert
        Assert.Contains(testDbContext.Shops, s => s.Id == shop.Id && s.TokenId == newToken.Id);
    }

    /// <summary>
    /// Verifies that an entity with a strongly-typed primary key can be deleted from the database.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_StronglyTypedPrimaryKey_Success()
    {
        // Arrange
        var token = Fakers.TokenFaker.Generate();
        testDbContext.Tokens.Add(token);
        await testDbContext.SaveChangesAsync(cancellationToken);

        // Act
        await efCoreDataService.DeleteAsync(token, typeof(Token), cancellationToken);

        // Assert
        Assert.DoesNotContain(testDbContext.Tokens, t => t.Id.Equals(token.Id));
    }
}

using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Demo.Net7;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Utilities;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Create entity tests.
/// </summary>
public class CreateEntityTests : IDisposable
{
    private readonly TestDatabaseFixture dbFixture;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests()
    {
        dbFixture = new TestDatabaseFixture();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        dbFixture.Dispose();
    }

    private static EfCoreDataService CreateEfCoreDataService(ShopDbContext shopDbContext)
    {
        var efCoreOptions = new EfCoreOptions();
        var shopDbContextType = typeof(ShopDbContext);
        efCoreOptions.DbContexts.Add(shopDbContextType);

        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider
            .Setup(provider => provider.GetService(shopDbContextType))
            .Returns(shopDbContext);

        return new EfCoreDataService(efCoreOptions, serviceProvider.Object);
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_ValidEntity_Success()
    {
        // Arrange
        await using var dbContext = dbFixture.CreateContext();
        var efCoreDataService = CreateEfCoreDataService(dbContext);

        var contactInfoType = typeof(ContactInfo);
        var contactInfo = new ContactInfo
        {
            Id = 3,
            Email = "Test3@test.test",
            FullName = "Test Contact3",
            PhoneNumber = "32223334455"
        };

        // Act
        await efCoreDataService.AddAsync(contactInfo, contactInfoType, CancellationToken.None);

        // Assert
        Assert.Contains(contactInfo, dbContext.ContactInfos);
    }

    /// <summary>
    /// Create already existed entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_AlreadyExistingEntity_Error()
    {
        // Arrange
        await using var dbContext = dbFixture.CreateContext();
        var efCoreDataService = CreateEfCoreDataService(dbContext);

        var contactInfoType = typeof(ContactInfo);
        var contactInfo = new ContactInfo
        {
            Id = 1,
            Email = "Test1@test.test",
            FullName = "Test Contact1",
            PhoneNumber = "12223334455"
        };

        // Act
        async Task Act() => await efCoreDataService.AddAsync(contactInfo, contactInfoType, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Act);
    }
}

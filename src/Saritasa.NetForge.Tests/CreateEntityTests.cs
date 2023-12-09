using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Demo.Net7;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Xunit;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Create entity tests.
/// </summary>
public class CreateEntityTests : IDisposable
{
    private ShopDbContext DbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests()
    {
        var dbOptions = new DbContextOptionsBuilder<ShopDbContext>()
            .UseInMemoryDatabase("NetForgeTest")
            .Options;

        DbContext = new ShopDbContext(dbOptions);
        DbContext.Database.EnsureCreated();

        DbContext.ContactInfos.Add(new ContactInfo
        {
            Id = 1,
            Email = "Test1@test.test",
            FullName = "Test Contact1",
            PhoneNumber = "12223334455"
        });
        DbContext.ContactInfos.Add(new ContactInfo
        {
            Id = 2,
            Email = "Test2@test.test",
            FullName = "Test Contact2",
            PhoneNumber = "22223334455"
        });
        DbContext.SaveChanges();
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
                DbContext.Database.EnsureDeleted();
            }

            disposedValue = true;
        }
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
        var efCoreDataService = CreateEfCoreDataService(DbContext);

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
        Assert.Contains(contactInfo, DbContext.ContactInfos);
    }

    /// <summary>
    /// Create already existed entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_AlreadyExistingEntity_Error()
    {
        // Arrange
        var efCoreDataService = CreateEfCoreDataService(DbContext);

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
        await Assert.ThrowsAnyAsync<InvalidOperationException>(Act);
    }
}

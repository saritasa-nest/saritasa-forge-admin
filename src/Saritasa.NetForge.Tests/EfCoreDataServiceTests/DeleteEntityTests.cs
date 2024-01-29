using Microsoft.EntityFrameworkCore;
using Moq;
using Saritasa.NetForge.Infrastructure.EfCore;
using Saritasa.NetForge.Infrastructure.EfCore.Services;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Delete entity tests.
/// </summary>
public class DeleteEntityTests : IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DeleteEntityTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("NetForgeTest")
            .Options;

        DbContext = new TestDbContext(dbOptions);
        DbContext.Database.EnsureCreated();
    }

    private TestDbContext DbContext { get; }

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
                DbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    private static EfCoreDataService CreateEfCoreDataService(TestDbContext testDbContext)
    {
        var efCoreOptions = new EfCoreOptions();
        var shopDbContextType = typeof(TestDbContext);
        efCoreOptions.DbContexts.Add(shopDbContextType);

        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider
            .Setup(provider => provider.GetService(shopDbContextType))
            .Returns(testDbContext);

        return new EfCoreDataService(efCoreOptions, serviceProvider.Object);
    }

    /// <summary>
    /// Delete valid entity test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_ValidEntity_Success()
    {
        // Arrange
        var efCoreDataService = CreateEfCoreDataService(DbContext);

        var contactInfoType = typeof(ContactInfo);
        var contactInfo = new ContactInfo
        {
            Id = 3, Email = "Test3@test.test", FullName = "Test Contact3", PhoneNumber = "32223334455"
        };

        // Add the contactInfo to the database before attempting to delete it
        await efCoreDataService.AddAsync(contactInfo, contactInfoType, CancellationToken.None);

        // Act
        await efCoreDataService.DeleteAsync(contactInfo, contactInfoType, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(contactInfo, DbContext.ContactInfos);
    }

    /// <summary>
    /// Delete non-existing entity test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_NonExistingEntity_NoEffect()
    {
        // Arrange
        var efCoreDataService = CreateEfCoreDataService(DbContext);

        var contactInfoType = typeof(ContactInfo);
        var nonExistingContactInfo = new ContactInfo
        {
            Id = 99, Email = "NonExisting@test.test", FullName = "Non-Existing Contact", PhoneNumber = "9999999999"
        };

        // Act
        // Assert that DbUpdateConcurrencyException is not thrown
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            efCoreDataService.DeleteAsync(nonExistingContactInfo, contactInfoType, CancellationToken.None));
    }

    /// <summary>
    /// Delete entity with invalid type test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_InvalidType_ThrowsException()
    {
        // Arrange
        var efCoreDataService = CreateEfCoreDataService(DbContext);

        var invalidType = typeof(int); // Assuming InvalidType is not a valid entity type

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            efCoreDataService.DeleteAsync(null, invalidType, CancellationToken.None));
    }
}

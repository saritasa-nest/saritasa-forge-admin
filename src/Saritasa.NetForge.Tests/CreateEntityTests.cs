using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;
using ContactInfo = Saritasa.NetForge.Tests.Domain.Models.ContactInfo;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Create entity tests.
/// </summary>
public class CreateEntityTests : IDisposable
{
    private TestDbContext DbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests()
    {
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new TestDbContext(dbOptions);
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
                DbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_ValidEntity_Success()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(DbContext);

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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(DbContext);

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

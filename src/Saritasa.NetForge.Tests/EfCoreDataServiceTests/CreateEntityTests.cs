using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;
using ContactInfo = Saritasa.NetForge.Tests.Domain.Models.ContactInfo;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Create entity tests.
/// </summary>
public class CreateEntityTests : IDisposable
{
    private TestDbContext TestDbContext { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests()
    {
        TestDbContext = EfCoreHelper.CreateTestDbContext();

        TestDbContext.ContactInfos.Add(new ContactInfo
        {
            Id = 1,
            Email = "Test1@test.test",
            FullName = "Test Contact1",
            PhoneNumber = "12223334455"
        });
        TestDbContext.ContactInfos.Add(new ContactInfo
        {
            Id = 2,
            Email = "Test2@test.test",
            FullName = "Test Contact2",
            PhoneNumber = "22223334455"
        });
        TestDbContext.SaveChanges();
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
                TestDbContext.Dispose();
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
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

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
        Assert.Contains(TestDbContext.ContactInfos, item => item.Id == contactInfo.Id);
    }

    /// <summary>
    /// Create already existed entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_AlreadyExistingEntity_Error()
    {
        // Arrange
        var efCoreDataService = EfCoreHelper.CreateEfCoreDataService(TestDbContext);

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

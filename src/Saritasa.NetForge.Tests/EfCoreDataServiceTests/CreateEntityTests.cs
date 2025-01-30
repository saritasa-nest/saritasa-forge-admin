using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
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
    private readonly TestDbContext testDbContext;
    private readonly IOrmDataService efCoreDataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests()
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
    /// Deletes the database after one test is complete,
    /// so it gives us the same state of the database for every test.
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
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_ValidEntity_Success()
    {
        // Arrange
        var contactInfoType = typeof(ContactInfo);
        var contactInfo = Fakers.ContactInfoFaker.Generate();

        // Act
        await efCoreDataService.AddAsync(contactInfo, contactInfoType, CancellationToken.None);

        // Assert
        Assert.Contains(testDbContext.ContactInfos, item => item.Id == contactInfo.Id);
    }

    /// <summary>
    /// Create already existed entity test.
    /// </summary>
    [Fact]
    public async Task CreateEntity_AlreadyExistingEntity_Error()
    {
        // Arrange
        var contactInfo = Fakers.ContactInfoFaker.Generate();
        testDbContext.ContactInfos.Add(contactInfo);
        await testDbContext.SaveChangesAsync(CancellationToken.None);
        var alreadyExistingContactInfo = Fakers.ContactInfoFaker.Generate();
        alreadyExistingContactInfo.Id = contactInfo.Id;

        // Act
        async Task Act() => await efCoreDataService.AddAsync(
            alreadyExistingContactInfo, typeof(ContactInfo), CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(Act);
    }

    /// <summary>
    /// Validates that custom action after entity create is changing the entity.
    /// </summary>
    [Fact]
    public async Task CreateEntity_CustomAction_ShouldUpdate()
    {
        // Arrange
        var contactInfoType = typeof(ContactInfo);
        var newContactInfo = Fakers.ContactInfoFaker.Generate();

        const string customActionEmail = "test@test.com";
        Action<IServiceProvider?, object> customAction = (_, contactInfo) =>
        {
            ((ContactInfo)contactInfo).Email = customActionEmail;
        };

        // Act
        await efCoreDataService.AddAsync(newContactInfo, contactInfoType, CancellationToken.None, customAction);

        // Assert
        Assert.Contains(testDbContext.ContactInfos, contactInfo => contactInfo.Email == customActionEmail);
    }
}

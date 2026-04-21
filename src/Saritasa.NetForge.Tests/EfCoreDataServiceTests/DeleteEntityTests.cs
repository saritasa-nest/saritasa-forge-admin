using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Fixtures;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Delete entity tests.
/// </summary>
public class DeleteEntityTests : IClassFixture<NetForgeFixture>
{
    private readonly TestDbContext testDbContext;
    private readonly IOrmDataService efCoreDataService;

    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DeleteEntityTests(NetForgeFixture fixture)
    {
        testDbContext = fixture.GetService<TestDbContext>();
        efCoreDataService = fixture.GetService<IOrmDataService>();
    }

    /// <summary>
    /// Delete valid entity test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_ValidEntity_Success()
    {
        // Arrange
        var contactInfoType = typeof(ContactInfo);
        var contactInfo = Fakers.ContactInfoFaker.Generate();

        // Add the contactInfo to the database before attempting to delete it
        testDbContext.Add(contactInfo);

        // Act
        await efCoreDataService.DeleteAsync(contactInfo, contactInfoType, cancellationToken);

        // Assert
        Assert.DoesNotContain(testDbContext.ContactInfos, item => item.Id == contactInfo.Id);
    }

    /// <summary>
    /// Delete non-existing entity test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_NonExistingEntity_NoEffect()
    {
        // Arrange
        var contactInfoType = typeof(ContactInfo);
        var nonExistingContactInfo = Fakers.ContactInfoFaker.Generate();

        // Act
        // Assert that DbUpdateConcurrencyException is thrown
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            efCoreDataService.DeleteAsync(nonExistingContactInfo, contactInfoType, cancellationToken));
    }

    /// <summary>
    /// Delete entity with invalid type test.
    /// </summary>
    [Fact]
    public async Task DeleteEntity_InvalidType_ThrowsException()
    {
        // Arrange
        var invalidType = typeof(int); // Assuming InvalidType is not a valid entity type

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            efCoreDataService.DeleteAsync(1, invalidType, cancellationToken));
    }
}

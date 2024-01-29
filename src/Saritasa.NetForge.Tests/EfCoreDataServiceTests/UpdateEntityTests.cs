using Microsoft.EntityFrameworkCore;
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
    private readonly TestDbContext testDbContext;
    private readonly IOrmDataService efCoreDataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UpdateEntityTests(ITestOutputHelper testOutputHelper, NetForgeFixture netForgeFixture)
        : base(testOutputHelper, netForgeFixture)
    {
        testDbContext = _fixture.GetService<TestDbContext>(_testOutputHelper)!;
        efCoreDataService = _fixture.GetService<IOrmDataService>(_testOutputHelper)!;

        var contacts = Fakers.ContactInfoFaker.Generate(2);
        testDbContext.ContactInfos.AddRange(contacts);
        testDbContext.SaveChanges();
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    public async Task UpdateEntity_ValidEntity_Success()
    {
        // Arrange
        var contactInfo = await testDbContext.ContactInfos.FirstAsync();

        const string newEmail = "Test222@test.test";
        contactInfo.Email = newEmail;

        // Act
        await efCoreDataService.UpdateAsync(contactInfo, CancellationToken.None);

        // Assert
        Assert.Contains(testDbContext.ContactInfos, contact => contact.Email.Equals(newEmail));
    }
}

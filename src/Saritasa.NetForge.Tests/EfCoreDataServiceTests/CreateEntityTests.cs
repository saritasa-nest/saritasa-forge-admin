using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Fixtures;
using Saritasa.NetForge.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Microsoft.DependencyInjection.Attributes;
using ContactInfo = Saritasa.NetForge.Tests.Domain.Models.ContactInfo;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests;

/// <summary>
/// Create entity tests.
/// </summary>
[TestCaseOrderer(Constants.OrdererTypeName, Constants.OrdererAssemblyName)]
public class CreateEntityTests : TestBed<NetForgeFixture>
{
#pragma warning disable CA2213
    private readonly TestDbContext testDbContext;
#pragma warning restore CA2213
    private readonly IOrmDataService efCoreDataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityTests(ITestOutputHelper testOutputHelper, NetForgeFixture netForgeFixture)
        : base(testOutputHelper, netForgeFixture)
    {
        testDbContext = _fixture.GetService<TestDbContext>(_testOutputHelper)!;
        efCoreDataService = _fixture.GetService<IOrmDataService>(_testOutputHelper)!;
    }

    /// <summary>
    /// Create valid entity test.
    /// </summary>
    [Fact]
    [TestOrder(1)]
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
    [TestOrder(2)]
    public async Task CreateEntity_AlreadyExistingEntity_Error()
    {
        // Arrange
        var contactInfo = Fakers.ContactInfoFaker.Generate();
        testDbContext.ContactInfos.Add(contactInfo);
        await testDbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        async Task Act() => await efCoreDataService.AddAsync(contactInfo, typeof(ContactInfo), CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(Act);
    }
}

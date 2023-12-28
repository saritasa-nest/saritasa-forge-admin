using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Utilities;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Services;
using Saritasa.Tools.Domain.Exceptions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Tests for <see cref="EntityService.GetEntityByIdAsync"/>.
/// </summary>
[CollectionDefinition(TestConstants.DependencyInjection)]
public class GetEntityByIdTests : TestBed<TestDatabaseFixture>
{
    private readonly IEntityService entityService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdTests(ITestOutputHelper testOutputHelper, TestDatabaseFixture testDatabaseFixture)
        : base(testOutputHelper, testDatabaseFixture)
    {
        entityService = testDatabaseFixture.GetService<IEntityService>(testOutputHelper)!;
    }

    /// <summary>
    /// Test for case when string id is valid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_ValidStringId_ShouldBeNotNull()
    {
        // Arrange
        const string stringId = "Addresses";

        // Act
        var entity = await entityService.GetEntityByIdAsync(stringId, CancellationToken.None);

        // Assert
        Assert.NotNull(entity);
    }

    /// <summary>
    /// Test for case when string id is invalid.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_InvalidStringId_ShouldThrowNotFoundException()
    {
        // Arrange
        const string stringId = "Addresses2";

        // Act
        var getEntityByIdCall = () => entityService.GetEntityByIdAsync(stringId, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(getEntityByIdCall);
    }

    ///// <summary>
    ///// Test for case when string id is invalid.
    ///// </summary>
    //[Fact]
    //public async Task GetEntityByIdAsync_InvalidStringId_ShouldBeNotNull()
    //{
    //    // Arrange
    //    const string stringId = "Addresses";

    //    // Act
    //    var entity = await entityService.GetEntityByIdAsync(stringId, CancellationToken.None);

    //    // Assert
    //    Assert.NotNull(entity.Properties.FirstOrDefault(property => property.Name.Equals(nameof(Address.Street))));
    //}
}

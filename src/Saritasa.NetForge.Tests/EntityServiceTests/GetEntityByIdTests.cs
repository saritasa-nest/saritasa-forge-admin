using Moq;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.NetForge.UseCases.Services;
using Saritasa.Tools.Domain.Exceptions;
using Xunit;

namespace Saritasa.NetForge.Tests.EntityServiceTests;

/// <summary>
/// Tests for <see cref="EntityService.GetEntityByIdAsync"/>.
/// </summary>
public class GetEntityByIdTests : IDisposable
{
    private readonly TestDbContext testDbContext;
    private readonly IEntityService entityService;
    private readonly AdminOptionsBuilder adminOptionsBuilder;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdTests()
    {
        testDbContext = EfCoreHelper.CreateTestDbContext();
        adminOptionsBuilder = new AdminOptionsBuilder();
        var adminMetadataService = new AdminMetadataService(
            EfCoreHelper.CreateEfCoreMetadataService(testDbContext),
            adminOptionsBuilder.Create(),
            MemoryCacheHelper.CreateMemoryCache());

        entityService = new EntityService(
            AutomapperHelper.CreateAutomapper(),
            adminMetadataService,
            EfCoreHelper.CreateEfCoreDataService(testDbContext),
            new Mock<IServiceProvider>().Object);
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
                testDbContext.Dispose();
            }

            disposedValue = true;
        }
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

    /// <summary>
    /// Test for case when navigation included to entity.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithNavigations_ShouldBeNotNull()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder.IncludeNavigations(entity => entity.Address);
        });

        const string stringId = "Shops";
        const string navigationPropertyName = nameof(Shop.Address);

        // Act
        var entity = await entityService.GetEntityByIdAsync(stringId, CancellationToken.None);

        // Assert
        Assert.Contains(entity.Properties, property => property.Name.Equals(navigationPropertyName));
    }

    /// <summary>
    /// Test for case when property excluded from query.
    /// </summary>
    [Fact]
    public async Task GetEntityByIdAsync_WithExcludedFromQueryProperty_ShouldNotContainExcludedProperty()
    {
        // Arrange
        adminOptionsBuilder.ConfigureEntity<Shop>(builder =>
        {
            builder
                .ConfigureProperty(shop => shop.IsOpen, optionsBuilder => optionsBuilder.SetIsExcludedFromQuery(true));
        });

        const string stringId = "Shops";
        const string hiddenPropertyName = nameof(Shop.IsOpen);

        // Act
        var entity = await entityService.GetEntityByIdAsync(stringId, CancellationToken.None);

        // Assert
        Assert.DoesNotContain(entity.Properties, property => property.Name.Equals(hiddenPropertyName));
    }
}

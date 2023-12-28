using Saritasa.NetForge.Tests.Domain;
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
    private readonly TestDbContext testDbContext;
    private readonly IEntityService entityService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdTests(ITestOutputHelper testOutputHelper, TestDatabaseFixture testDatabaseFixture)
        : base(testOutputHelper, testDatabaseFixture)
    {
        testDbContext = testDatabaseFixture.TestDbContext;
        entityService = testDatabaseFixture.GetService<IEntityService>(testOutputHelper)!;

        PopulateDatabaseWithTestData();
    }

    private void PopulateDatabaseWithTestData()
    {
        if (testDbContext.Addresses.Any())
        {
            return;
        }

        testDbContext.Addresses.Add(new Address
        {
            Street = "Main St.",
            City = "New York",
            Latitude = 100
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Main Square St.",
            City = "London",
            Latitude = 101
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Second Square St.",
            City = "London",
            Latitude = 102
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Second main St.",
            City = "New York",
            Latitude = 10
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "Central",
            City = "London",
            Latitude = 222
        });
        testDbContext.Addresses.Add(new Address
        {
            Street = "central street",
            City = "New York",
            Latitude = 1
        });

        testDbContext.Products.Add(new Product
        {
            Supplier = new Supplier()
        });
        testDbContext.Products.Add(new Product
        {
            WeightInGrams = 111,
            Supplier = new Supplier
            {
                Name = "Supplier",
                City = "London"
            }
        });
        testDbContext.SaveChanges();
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
}

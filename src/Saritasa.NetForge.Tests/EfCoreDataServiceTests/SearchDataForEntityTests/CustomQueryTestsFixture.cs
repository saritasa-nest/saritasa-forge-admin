using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Fixtures;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests.SearchDataForEntityTests;

/// <summary>
/// Shared fixture for <see cref="CustomQueryTests"/> that populates the database once for all tests.
/// </summary>
public class CustomQueryTestsFixture : NetForgeFixture
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CustomQueryTestsFixture()
    {
        var testDbContext = GetService<TestDbContext>();

        testDbContext.Shops.Add(new Shop
        {
            Name = "London Shop",
            Suppliers = new List<Supplier>
            {
                new() { Name = "London Supplier", City = "London" }
            }
        });

        testDbContext.Shops.Add(new Shop
        {
            Name = "Paris Shop",
            Suppliers = new List<Supplier>
            {
                new() { Name = "Paris Supplier", City = "Paris" }
            }
        });

        testDbContext.Shops.Add(new Shop
        {
            Name = "Empty Shop"
        });

        testDbContext.SaveChanges();
    }
}


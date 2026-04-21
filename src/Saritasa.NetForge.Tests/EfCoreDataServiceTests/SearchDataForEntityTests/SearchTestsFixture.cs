using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Fixtures;

namespace Saritasa.NetForge.Tests.EfCoreDataServiceTests.SearchDataForEntityTests;

/// <summary>
/// Shared fixture for <see cref="SearchTests"/> that populates the database once for all tests.
/// </summary>
public class SearchTestsFixture : NetForgeFixture
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchTestsFixture()
    {
        var testDbContext = GetService<TestDbContext>();

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
        testDbContext.Products.Add(new Product
        {
            WeightInGrams = 222,
            Supplier = new Supplier
            {
                Name = "ABC",
                City = "New-York"
            }
        });
        testDbContext.SaveChanges();
    }
}


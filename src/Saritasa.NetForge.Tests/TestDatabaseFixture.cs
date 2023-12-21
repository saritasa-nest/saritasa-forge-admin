using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Tests.Domain;
using Saritasa.NetForge.Tests.Domain.Models;
using Saritasa.NetForge.Tests.Helpers;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests;

/// <summary>
/// Test database fixture.
/// </summary>
/// <remarks>
/// Use case:
/// When we want the same database state for all tests in one class.
/// For example, when we don't modify any data in the database.
/// </remarks>
public class TestDatabaseFixture : TestBedFixture
{
    internal TestDbContext TestDbContext { get; set; }

    /// <summary>
    /// Constructor. This code executes before tests.
    /// </summary>
    public TestDatabaseFixture()
    {
        TestDbContext = EfCoreHelper.CreateTestDbContext();

        TestDbContext.Addresses.Add(new Address
        {
            Street = "Main St.",
            City = "New York",
            Latitude = 100
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Main Square St.",
            City = "London",
            Latitude = 101
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Second Square St.",
            City = "London",
            Latitude = 102
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Second main St.",
            City = "New York",
            Latitude = 10
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "Central",
            City = "London",
            Latitude = 222
        });
        TestDbContext.Addresses.Add(new Address
        {
            Street = "central street",
            City = "New York",
            Latitude = 1
        });

        TestDbContext.Products.Add(new Product
        {
            Supplier = new Supplier()
        });
        TestDbContext.Products.Add(new Product
        {
            WeightInGrams = 111,
            Supplier = new Supplier
            {
                Name = "Supplier",
                City = "London"
            }
        });
        TestDbContext.SaveChanges();
    }

    /// <inheritdoc />
    protected override ValueTask DisposeAsyncCore()
    {
        TestDbContext.Dispose();

        return default;
    }

    /// <inheritdoc />
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        AutoMapperModule.Register(services);

        services.AddDbContext<TestDbContext>(options =>
        {
            options.UseInMemoryDatabase("NetForgeTest");
        });

        services.AddNetForge(optionsBuilder =>
        {
            optionsBuilder.UseEntityFramework(efOptionsBuilder =>
            {
                efOptionsBuilder.UseDbContext<TestDbContext>();
            });
        });
    }

    /// <inheritdoc />
    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
    }
}

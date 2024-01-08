using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Blazor.Infrastructure.DependencyInjection;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Tests.Domain;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests.Utilities;

/// <summary>
/// Test database fixture.
/// </summary>
/// <remarks>
/// Use case:
/// When we want the same database state for all tests.
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
        var dbOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("NetForgeTest")
            .Options;

        var testDbContext = new TestDbContext(dbOptions);
        testDbContext.Database.EnsureCreated();

        TestDbContext = testDbContext;
    }

    /// <inheritdoc />
    protected override ValueTask DisposeAsyncCore() => default;

    /// <inheritdoc />
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        AutoMapperModule.Register(services);

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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Tests.Domain;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Saritasa.NetForge.Tests.Fixtures;

/// <summary>
/// Sets up an in-memory database and adds the NetForge library to the service collection.
/// </summary>
public class NetForgeFixture : TestBedFixture
{
    internal TestDbContext TestDbContext { get; set; }

    /// <summary>
    /// Constructor. This code executes before tests.
    /// </summary>
    public NetForgeFixture()
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
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("NetForgeTest"));

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

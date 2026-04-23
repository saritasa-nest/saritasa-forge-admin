using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Tests.Domain;

namespace Saritasa.NetForge.Tests.Fixtures;

/// <summary>
/// Sets up an in-memory database and adds the NetForge library to the service collection.
/// </summary>
public class NetForgeFixture : IDisposable
{
    private readonly ServiceProvider serviceProvider;
    private bool disposedValue;

    /// <summary>
    /// Constructor.
    /// </summary>
    public NetForgeFixture()
    {
        var services = new ServiceCollection();

        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddNetForge(optionsBuilder =>
        {
            optionsBuilder.UseEntityFramework(efOptionsBuilder =>
            {
                efOptionsBuilder.UseDbContext<TestDbContext>();
            });
        });

        serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Resolves a service from the fixture's service provider.
    /// </summary>
    public T GetService<T>() where T : notnull => serviceProvider.GetRequiredService<T>();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases managed resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            serviceProvider.Dispose();
        }

        disposedValue = true;
    }
}

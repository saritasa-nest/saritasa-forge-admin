using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore.Services;

namespace Saritasa.NetForge.Infrastructure.EfCore;

/// <summary>
/// Builds EF Core specific options for the admin panel.
/// </summary>
public class EfCoreOptionsBuilder : IOrmOptionsBuilder
{
    /// <summary>
    /// EF Core specific options.
    /// </summary>
    public EfCoreOptions Options { get; set; } = new();

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(Options);
        services.AddScoped<IMetadataService, MetadataService>();
    }

    /// <summary>
    /// Adds DB context to use in the panel.
    /// </summary>
    /// <typeparam name="TDbContext">Type of EF Core DB context.</typeparam>
    /// <returns>The instance of the options builder.</returns>
    public EfCoreOptionsBuilder UseDbContext<TDbContext>() where TDbContext : DbContext
    {
        Options.DbContexts.Add(typeof(TDbContext));
        return this;
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Provides a contract for ORM-specific option builders.
/// </summary>
public interface IOrmOptionsBuilder
{
    /// <summary>
    /// Injects ORM-specific services into the admin panel.
    /// </summary>
    /// <param name="services">Collection of application services.</param>
    void ApplyServices(IServiceCollection services);
}

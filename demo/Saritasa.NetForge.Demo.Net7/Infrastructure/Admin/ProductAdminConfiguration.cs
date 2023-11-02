using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;

/// <summary>
/// <see cref="Product"/> admin panel configuration.
/// </summary>
public class ProductAdminConfiguration : IEntityAdminConfiguration<Product>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Product> entityOptionsBuilder)
    {
        entityOptionsBuilder
            .AddCalculatedProperties(entity => entity.VolumeInCentimeters, entity => entity.Name);
    }
}
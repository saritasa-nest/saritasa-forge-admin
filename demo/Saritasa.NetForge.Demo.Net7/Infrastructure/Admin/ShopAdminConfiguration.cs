using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;

/// <summary>
/// <see cref="Shop"/> admin panel configuration.
/// </summary>
public class ShopAdminConfiguration : IEntityAdminConfiguration<Shop>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
    {
        entityOptionsBuilder
            .SetDescription("The base Shop entity.")
            .ConfigureSearch((serviceProvider, query, searchTerm) =>
            {
                return query.Where(e => e.Name.Contains(searchTerm));
            });

        entityOptionsBuilder.ConfigureProperty(shop => shop.IsOpen, builder =>
        {
            builder.SetIsSortable(true);
        });

        entityOptionsBuilder.ConfigureProperty(shop => shop.OpenedDate, builder =>
        {
            builder.SetIsSortable(true);
        });

        entityOptionsBuilder
            .IncludeNavigations(
                shop => shop.Address,
                shop => shop.OwnerContact,
                shop => shop.Products,
                shop => shop.Suppliers)
            .ConfigureProperty(shop => shop.OwnerContact, builder =>
            {
                builder
                .SetDisplayName("OwnerContactInfo")
                .SetDescription("Information about owner contact.")
                .SetOrder(2)
                .SetEmptyValueDisplay("N/A");
            });
    }
}

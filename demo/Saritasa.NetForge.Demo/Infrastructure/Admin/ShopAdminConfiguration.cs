using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.DomainServices;
using Saritasa.NetForge.DomainServices.Interfaces;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

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
            .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .IncludeProperty(address => address.Street, builder =>
                    {
                        builder.SetDisplayName("Address Street");
                    })
                    .IncludeProperty(address => address.Id);
            })
            .IncludeNavigation<ContactInfo>(shop => shop.OwnerContact, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .IncludeProperty(contact => contact.Id)
                    .IncludeProperty(contact => contact.FullName);
            });
    }
}

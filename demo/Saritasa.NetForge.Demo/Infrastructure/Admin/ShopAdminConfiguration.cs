using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.Domain.Enums;
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
                    .IncludeProperty(address => address.Id, builder =>
                    {
                        builder.SetDisplayName("Address Id");
                    })
                    .IncludeProperty(address => address.Street, builder =>
                    {
                        builder
                            .SetDisplayName("Address Street")
                            .SetDescription("Address street name.")
                            .SetEmptyValueDisplay("N/A")
                            .SetIsSortable(true)
                            .SetSearchType(SearchType.ContainsCaseInsensitive);
                    });
            })
            .IncludeNavigation<ContactInfo>(shop => shop.OwnerContact, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .IncludeProperty(contact => contact.Id)
                    .IncludeProperty(contact => contact.FullName);
            });
    }
}

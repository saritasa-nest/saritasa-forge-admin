using Saritasa.NetForge.Demo.Infrastructure.UploadFiles;
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
            builder
                .SetIsSortable(true)
                .SetSearchType(SearchType.ContainsCaseInsensitive);
        });

        entityOptionsBuilder
            .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .SetOrder(1)
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
            .IncludeNavigation<Product>(shop => shop.Products, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder.IncludeProperty(product => product.Id);
            })
            .IncludeNavigation<Supplier>(shop => shop.Suppliers, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .IncludeProperty(supplier => supplier.Name, propertyOptionsBuilder =>
                    {
                        propertyOptionsBuilder.SetDisplayName("Supplier Name");
                    })
                    .IncludeProperty(supplier => supplier.City, propertyOptionsBuilder =>
                    {
                        propertyOptionsBuilder.SetDisplayName("Supplier City");
                    });
            });

        entityOptionsBuilder.ConfigureProperty(shop => shop.Logo, builder =>
        {
            builder
                .SetIsImagePath(true)
                .SetImageFolder("Shop images")
                .SetOrder(3)
                .SetUploadFileStrategy(new UploadFileToFileSystemStrategy());
        });

        entityOptionsBuilder.ConfigureProperty(shop => shop.BuildingPhoto, builder =>
        {
            builder.SetUploadFileStrategy(new UploadBase64FileStrategy());
        });

        entityOptionsBuilder.ConfigureProperty(shop => shop.Name, builder =>
        {
            builder.SetTruncationMaxCharacters(25);
        });
    }
}

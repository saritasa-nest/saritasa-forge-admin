using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Interfaces;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Demo.Infrastructure.UploadFiles.Strategies;
using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

/// <summary>
/// <see cref="Product"/> admin panel configuration.
/// </summary>
public class ProductAdminConfiguration : IEntityAdminConfiguration<Product>
{
    private readonly ServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProductAdminConfiguration(IServiceCollection services)
    {
        serviceProvider = services.BuildServiceProvider();
    }

    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Product> entityOptionsBuilder)
    {
        entityOptionsBuilder.IncludeNavigation<Supplier>(product => product.Supplier, navigationOptionsBuilder =>
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

        entityOptionsBuilder.IncludeNavigation<Shop>(product => product.Shop, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder
                .IncludeProperty(shop => shop.Id, propertyOptionsBuilder =>
                {
                    propertyOptionsBuilder.SetDisplayName("Shop Id");
                })
                .IncludeProperty(shop => shop.Logo, propertyOptionsBuilder =>
                {
                    var s3Storage = serviceProvider.GetRequiredService<IBlobStorageService>();
                    var cloudStorage = serviceProvider.GetRequiredService<ICloudBlobStorageService>();
                    propertyOptionsBuilder
                        .SetIsImage(true)
                        .SetUploadFileStrategy(new UploadFileToS3Strategy(s3Storage, cloudStorage));
                })
                .IncludeProperty(shop => shop.BuildingPhoto, propertyOptionsBuilder =>
                {
                    propertyOptionsBuilder
                        .SetIsImage(true)
                        .SetUploadFileStrategy(new UploadBase64FileStrategy());
                })
                .IncludeNavigation<ContactInfo>(shop => shop.OwnerContact, builder =>
                {
                    builder.IncludeProperty(contactInfo => contactInfo.Email, propertyBuilder =>
                    {
                        propertyBuilder
                            .SetOrder(1)
                            .SetDisplayName("Contact Email")
                            .SetSearchType(SearchType.StartsWithCaseSensitive)
                            .SetIsSortable(true);
                    });
                })
                .IncludeNavigation<Address>(shop => shop.Address, builder =>
                {
                    builder.IncludeProperty(address => address.Country, propertyBuilder =>
                    {
                        propertyBuilder
                            .SetOrder(2)
                            .SetDisplayName("Shop Country");
                    });
                })
                .IncludeNavigation<Supplier>(shop => shop.Suppliers, builder =>
                {
                    builder
                        .IncludeProperty(supplier => supplier.Name, propertyOptionsBuilder =>
                        {
                            propertyOptionsBuilder.SetDisplayName("Supplier Name");
                        })
                        .IncludeProperty(supplier => supplier.City, propertyOptionsBuilder =>
                        {
                            propertyOptionsBuilder.SetDisplayName("Supplier City");
                        });
                });
        });

        entityOptionsBuilder.ConfigureProperty(product => product.UpdatedDate, builder =>
        {
            builder.SetIsReadOnly(true);
        });
    }
}
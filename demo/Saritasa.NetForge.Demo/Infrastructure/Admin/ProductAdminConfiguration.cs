using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.DomainServices.Interfaces;
using Saritasa.NetForge.DomainServices;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

/// <summary>
/// <see cref="Product"/> admin panel configuration.
/// </summary>
public class ProductAdminConfiguration : IEntityAdminConfiguration<Product>
{
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
                    propertyOptionsBuilder.SetIsImagePath(true);
                })
                .IncludeProperty(shop => shop.BuildingPhoto, propertyOptionsBuilder =>
                {
                    propertyOptionsBuilder.SetIsBase64Image(true);
                });
        });

        entityOptionsBuilder.ConfigureProperty(product => product.UpdatedDate, builder =>
        {
            builder.SetIsReadOnly(true);
        });
    }
}
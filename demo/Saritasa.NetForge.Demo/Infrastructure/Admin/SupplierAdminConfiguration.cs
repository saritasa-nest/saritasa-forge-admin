using Saritasa.NetForge.Demo.Infrastructure.UploadFiles.Strategies;
using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Interfaces;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

public class SupplierAdminConfiguration : IEntityAdminConfiguration<Supplier>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Supplier> entityOptionsBuilder)
    {
        entityOptionsBuilder.ConfigureProperty(supplier => supplier.Name, builder =>
        {
            builder
                .SetOrder(1)
                .SetFormOrder(1);
        });

        entityOptionsBuilder.ConfigureProperty(supplier => supplier.City, builder =>
        {
            builder
                .SetOrder(2)
                .SetFormOrder(2);
        });

        entityOptionsBuilder.ConfigureProperty(supplier => supplier.IsActive, builder =>
        {
            builder
                .SetOrder(3)
                .SetFormOrder(3);
        });

        entityOptionsBuilder.IncludeNavigation<Director>(shop => shop.Director, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder.IncludeProperty(director => director.Name, builder =>
            {
                builder
                    .SetOrder(4)
                    .SetFormOrder(4);
            });

            navigationOptionsBuilder.IncludeProperty(director => director.Description, builder =>
            {
                builder
                    .SetOrder(5)
                    .SetFormOrder(5)
                    .SetIsRichTextField(true);
            });

            navigationOptionsBuilder.IncludeProperty(director => director.Photo, builder =>
            {
                builder
                    .SetIsImage(true)
                    .SetUploadFileStrategy(new UploadBase64FileStrategy());
            });

            navigationOptionsBuilder.IncludeNavigation<Address>(director => director.Address, builder =>
            {
                builder.IncludeProperty(address => address.Street, streetBuilder =>
                {
                });
            });
        });
    }
}
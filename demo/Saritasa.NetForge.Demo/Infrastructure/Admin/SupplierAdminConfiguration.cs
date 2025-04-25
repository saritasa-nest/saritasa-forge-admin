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
        entityOptionsBuilder.IncludeNavigation<Director>(shop => shop.Director, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder.IncludeProperty(director => director.Name, builder =>
            {
                builder.SetDisplayName("Supplier Director");
            });

            navigationOptionsBuilder.IncludeProperty(director => director.Description, builder =>
            {
                builder
                    .SetDisplayName("Director Description")
                    .SetIsRichTextField(true);
            });

            navigationOptionsBuilder.IncludeProperty(director => director.Photo, builder =>
            {
                builder
                    .SetDisplayName("Director Photo")
                    .SetIsImage(true)
                    .SetUploadFileStrategy(new UploadBase64FileStrategy());
            });
        });
    }
}
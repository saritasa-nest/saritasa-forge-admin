using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;
using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Demo.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Infrastructure.Extensions;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

/// <summary>
/// Register NetForge admin panel as dependency.
/// </summary>
internal static class NetForgeModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddNetForge(optionsBuilder =>
        {
            optionsBuilder.UseEntityFramework(efOptionsBuilder => { efOptionsBuilder.UseDbContext<ShopDbContext>(); })
                .AddGroups(new List<EntityGroup>
                {
                    new() { Name = GroupConstants.Identity, Description = GroupConstants.IdentityDescription },
                    new() { Name = GroupConstants.Shops }
                })
                .SetGroupHeadersExpanded(true)
                .SetEntityCreateMessage("The entity was created.")
                .SetEntitySaveMessage("The entity was saved.")
                .SetEntityDeleteMessage("The entity was deleted.")
                .SetEntityBulkDeleteMessage("The entities were deleted.")
                .ConfigureEntity(new ShopAdminConfiguration(services))
                .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
                {
                    entityOptionsBuilder.SetIsHidden(true);
                })
                .AddIdentityGroup()
                .ConfigureEntity(new UserAdminConfiguration())
                .ConfigureEntity(new AddressAdminConfiguration())
                .ConfigureEntity(new ProductAdminConfiguration(services))
                .ConfigureEntity<ShopProductsCount>(entityOptionsBuilder =>
                {
                    entityOptionsBuilder.IncludeNavigation<Shop>(
                        shopProductsCount => shopProductsCount.Shop,
                        navigationOptionsBuilder =>
                        {
                            navigationOptionsBuilder
                                .IncludeProperty(shop => shop.Name, propertyOptionsBuilder =>
                                    {
                                        propertyOptionsBuilder.SetDisplayName("Shop name");
                                    });
                        });
                })
                .ConfigureEntity(new ContactInfoAdminConfiguration());
        });
    }
}
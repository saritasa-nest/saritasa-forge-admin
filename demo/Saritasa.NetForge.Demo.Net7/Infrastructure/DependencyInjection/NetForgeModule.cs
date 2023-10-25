using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.DependencyInjection;

/// <summary>
/// Register NetForge admin panel as dependency.
/// </summary>
internal class NetForgeModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    public static void Register(IServiceCollection services)
    {
        services.AddNetForge(optionsBuilder =>
        {
            optionsBuilder.UseEntityFramework(efOptionsBuilder =>
            {
                efOptionsBuilder.UseDbContext<ShopDbContext>();
            });
            optionsBuilder.ConfigureEntity<Shop>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetDescription("The base Shop entity.");
            });
            optionsBuilder.ConfigureEntity<ProductTag>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetIsHidden(true);
            });
            optionsBuilder.ConfigureEntity<Address>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.ConfigureProperty(address => address.Id, propertyBuilder =>
                {
                    propertyBuilder
                        .SetDescription("Item identifier.")
                        .SetOrder(2);
                });

                entityOptionsBuilder.ConfigureProperty(address => address.ContactPhone, propertyBuilder =>
                {
                    propertyBuilder
                        .SetDisplayName("Phone")
                        .SetDescription("Address contact phone.")
                        .SetOrder(1);
                });

                entityOptionsBuilder.ConfigureProperty(address => address.PostalCode, propertyBuilder =>
                {
                    propertyBuilder.SetIsHidden(true);
                });

                entityOptionsBuilder.ConfigureProperty(address => address.City, propertyBuilder =>
                {
                    propertyBuilder.SetDisplayName("Town");
                });

                entityOptionsBuilder.ConfigureProperty(address => address.Longitude, propertyBuilder =>
                {
                    propertyBuilder.SetOrder(6);
                });

                entityOptionsBuilder.ConfigureProperty(address => address.Country, propertyBuilder =>
                {
                    propertyBuilder.SetIsSearchable(true);
                });
            });
        });
    }
}
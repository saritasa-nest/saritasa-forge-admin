using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Demo.Net7.Infrastructure.DependencyInjection;

/// <summary>
/// Register NetForge admin panel as dependency.
/// </summary>
internal static class NetForgeModule
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
            })
            .AddGroup("Test Name", "Test Description")
            .AddGroup("Test Name 1", "Test Description 1")
            .AddGroup("Test Empty")
            .ConfigureEntity<Shop>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Test Name");
                entityOptionsBuilder.SetDescription("The base Shop entity.");
            })
            .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetIsHidden(true);
            }).ConfigureEntity(new AddressAdminConfiguration())
            .ConfigureEntity<Product>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.ConfigureProperty(product => product.Name, builder =>
                {
                    builder.SetSearchType(SearchType.ExactMatchCaseInsensitive);
                });
            });
        });
    }
}
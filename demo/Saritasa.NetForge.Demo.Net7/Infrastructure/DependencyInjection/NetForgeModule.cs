using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Constants;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Extensions;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Domain.Entities.Options;
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
            optionsBuilder.ConfigureAuth(async provider =>
            {
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                if (context is null)
                {
                    return false;
                }

                // Check if user is authenticated.
                var user = context.User;
                var userIsAnonymous =
                    user?.Identity == null || user.Identities.Any(i => i.IsAuthenticated == false);

                return !userIsAnonymous;
            });

            optionsBuilder.UseEntityFramework(efOptionsBuilder => { efOptionsBuilder.UseDbContext<ShopDbContext>(); })
                .AddGroups(new List<EntityGroup>
                {
                    new() { Name = GroupConstants.Identity, Description = GroupConstants.IdentityDescription },
                    new() { Name = GroupConstants.Shops }
                }).ConfigureEntity(new ShopAdminConfiguration())
                .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
                {
                    entityOptionsBuilder.SetIsHidden(true);
                }).AddIdentityGroup()
                .ConfigureEntity(new UserAdminConfiguration())
                .ConfigureEntity(new AddressAdminConfiguration());
        });
    }
}
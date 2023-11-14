using Saritasa.NetForge.Blazor.Extensions;
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
    private const string IdentityGroupName = "Identity";
    private const string IdentityGroupDescription = "Managing user identity within the system";
    private const string ShopGroupName = "Shops";

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
                }).AddGroups(new List<EntityGroup>
                {
                    new() { Name = IdentityGroupName, Description = IdentityGroupDescription },
                    new() { Name = ShopGroupName }
                }).ConfigureEntity(new ShopAdminConfiguration())
                .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
                {
                    entityOptionsBuilder.SetIsHidden(true);
                }).AddIdentityGroup();
        });
    }
}
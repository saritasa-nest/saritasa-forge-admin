using Microsoft.AspNetCore.Identity;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;
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
            optionsBuilder.UseEntityFramework(efOptionsBuilder =>
            {
                efOptionsBuilder.UseDbContext<ShopDbContext>();
            }).AddGroups(new List<EntityGroup>
            {
                new EntityGroup{ Name = "Identity", Description = "Managing user identity within the system" },
                new EntityGroup{ Name = "Shops"}
            }).ConfigureEntity(new ShopAdminConfiguration())
            .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetIsHidden(true);
            }).ConfigureEntity<IdentityRole>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<User>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<IdentityRoleClaim<string>>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<IdentityUserClaim<string>>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<IdentityUserLogin<string>>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<IdentityUserRole<string>>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity<IdentityUserToken<string>>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetGroup("Identity");
            }).ConfigureEntity(new UserAdminConfiguration());
        });
    }
}
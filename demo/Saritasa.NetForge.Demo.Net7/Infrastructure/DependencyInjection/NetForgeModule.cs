using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Net7.Models;
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
                new EntityGroup{ Name = "Group Entities 1", Description = "Group Description" },
                new EntityGroup{ Name = "Group Entities 2", Description = "Group Description" },
                new EntityGroup{ Name = "Group Entities 3"}
            }).ConfigureEntity<Shop>(entityOptionsBuilder =>
            {
                entityOptionsBuilder
                    .SetDescription("The base Shop entity.")
                    .ConfigureSearch((serviceProvider, query, searchTerm) =>
                    {
                        return query.Where(e => e.Name.Contains(searchTerm));
                    });
                    .SetGroup("Group Entities 1");
                    .SetDescription("The base Shop entity.");
            }).ConfigureEntity<ProductTag>(entityOptionsBuilder =>
            {
                entityOptionsBuilder.SetIsHidden(true);
            }).ConfigureEntity(new UserAdminConfiguration());
        });
    }
}
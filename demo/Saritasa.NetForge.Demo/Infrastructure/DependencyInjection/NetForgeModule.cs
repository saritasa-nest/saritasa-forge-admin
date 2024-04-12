using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Demo.Infrastructure.Admin;
using Saritasa.NetForge.Demo.Infrastructure.Extensions;
using Saritasa.NetForge.Demo.Infrastructure.UploadFiles.S3Storage;
using Saritasa.NetForge.Demo.Models;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

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
        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));

        services.AddScoped<IBlobStorageService, S3StorageService>();
        services.AddScoped<ICloudBlobStorageService, S3StorageService>();

        services.AddNetForge(optionsBuilder =>
        {
            optionsBuilder.ConfigureAuth(serviceProvider =>
            {
                var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                return Task.FromResult(httpContext?.User.Identity?.IsAuthenticated ?? false);
            });

            optionsBuilder.UseEntityFramework(efOptionsBuilder => { efOptionsBuilder.UseDbContext<ShopDbContext>(); })
                .AddGroups(new List<EntityGroup>
                {
                    new() { Name = GroupConstants.Identity, Description = GroupConstants.IdentityDescription },
                    new() { Name = GroupConstants.Shops }
                })
                .SetGroupHeadersExpanded(true)
                .ConfigureEntity(new ShopAdminConfiguration(services))
                .ConfigureEntity<ProductTag>(entityOptionsBuilder =>
                {
                    entityOptionsBuilder.SetIsHidden(true);
                }).AddIdentityGroup()
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
                                })
                                .SetOrder(1);
                        });
                });
        });
    }
}
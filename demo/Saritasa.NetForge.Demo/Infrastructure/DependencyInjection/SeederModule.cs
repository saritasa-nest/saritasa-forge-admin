using Saritasa.NetForge.Demo.Infrastructure.Seeders;

namespace Saritasa.NetForge.Demo.Infrastructure.DependencyInjection;

/// <summary>
/// Register seeders.
/// </summary>
internal static class SeederModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ISeeder, UsersSeeder>();
        services.AddScoped<ISeeder, AddressesSeeder>();
        services.AddScoped<ISeeder, ProductTagsSeeder>();
        services.AddScoped<ISeeder, ContactInfoSeeder>();
        services.AddScoped<ISeeder, ShopsSeeder>();
        services.AddScoped<ISeeder, ProductsSeeder>();
    }
}

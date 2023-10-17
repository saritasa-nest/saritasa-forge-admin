using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Startup;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Startup.HealthCheck;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Demo.Net7;

/// <summary>
/// Entry point for ASP.NET Core app.
/// </summary>
public class Startup
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Entry point for web application.
    /// </summary>
    /// <param name="configuration">Global configuration.</param>
    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Configure application services on startup.
    /// </summary>
    /// <param name="services">Services to configure.</param>
    /// <param name="environment">Application environment.</param>
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("AppDatabase")
                               ?? throw new ArgumentNullException("ConnectionStrings:AppDatabase",
                                   "Database connection string is not initialized");
        
        services.AddDbContext<ShopDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
        services.AddAsyncInitializer<DatabaseInitializer>();
        services.AddHealthChecks().AddNpgSql(connectionString);
        
        // Identity.
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ShopDbContext>()
            .AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(new IdentityOptionsSetup().Setup);
        
        // Add NetForge admin panel.
        Infrastructure.DependencyInjection.NetForgeModule.Register(services);
    }

    /// <summary>
    /// Configure web application.
    /// </summary>
    /// <param name="app">Application builder.</param>
    public void Configure(WebApplication app)
    {
        app.UseNetForge();
        HealthCheckModule.Register(app);
    }
}

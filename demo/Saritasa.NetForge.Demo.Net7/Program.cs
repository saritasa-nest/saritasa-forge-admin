using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Startup;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Startup.HealthCheck;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AppDatabase")
                               ?? throw new ArgumentNullException("ConnectionStrings:AppDatabase",
                                   "Database connection string is not initialized");

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});
builder.Services.AddAsyncInitializer<DatabaseInitializer>();
builder.Services.AddHealthChecks().AddNpgSql(connectionString);

// Register NetForge.
builder.Services.AddNetForge(optionsBuilder =>
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
});

var app = builder.Build();
app.UseNetForge();
HealthCheckModule.Register(app);
app.Run();

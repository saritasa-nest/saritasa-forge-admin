using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.AspNetCore.Extensions;
using Saritasa.NetForge.Demo.NetCore7.DbContexts;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var appDatabaseConnectionString = builder.Configuration.GetConnectionString("AppDatabase")
                               ?? throw new ArgumentNullException("ConnectionStrings:AppDatabase",
                                   "Database connection string is not initialized");
var identityDatabaseConnectionString = builder.Configuration.GetConnectionString("IdentityDatabase")
                                  ?? throw new ArgumentNullException("ConnectionStrings:IdentityDatabase",
                                      "Database connection string is not initialized");

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(appDatabaseConnectionString);
});
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(identityDatabaseConnectionString);
});

// Register NetForge.
builder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEntityFramework(efOptionsBuilder =>
    {
        efOptionsBuilder.UseDbContext<ShopDbContext>();
        efOptionsBuilder.UseDbContext<AppIdentityDbContext>();
    });
    optionsBuilder.UseEndpoint("/admin");
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseNetForge();

app.Run();

using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.AspNetCore.Extensions;
using Saritasa.NetForge.Demo.NetCore7.DbContexts;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMvc();
builder.Services.AddServerSideBlazor();

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
app.MapControllers();

app.UseNetForge();

app.Run();

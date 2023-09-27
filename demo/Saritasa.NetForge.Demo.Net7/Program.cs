using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Blazor.Extensions;
using Saritasa.NetForge.Demo.Net7;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appDatabaseConnectionString = builder.Configuration.GetConnectionString("AppDatabase")
                               ?? throw new ArgumentNullException("ConnectionStrings:AppDatabase",
                                   "Database connection string is not initialized");

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(appDatabaseConnectionString);
});

// Register NetForge.
builder.Services.AddNetForge(optionsBuilder =>
{
     optionsBuilder.UseEntityFramework(efOptionsBuilder =>
     {
         efOptionsBuilder.UseDbContext<ShopDbContext>();
     });
     optionsBuilder.UseEndpoint("/manage");
     optionsBuilder.ConfigureEntity<Shop>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetDescription("The base Shop entity.");
     });
     optionsBuilder.ConfigureEntity<Address>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetPluralName("Addresses");
     });
     optionsBuilder.ConfigureEntity<ContactInfo>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetIsHidden(true);
     });
     optionsBuilder.ConfigureEntity<ProductTag>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetPluralName("Product Tags");
     });
});

var app = builder.Build();
app.UseNetForge();
app.Run();

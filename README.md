# NetForge - Admin Panel for ASP.NET Core 6 & 7

The NetForge is a library that provides a user-friendly and intuitive user interface for performing CRUD operations on your database entities within your .NET Core 6 and 7 applications.

## How to use

Add NetForge to your service collection in Program.cs within the MVC and ServerSideBlazor:

```csharp
appBuilder.Services.AddMvc();
appBuilder.Services.AddServerSideBlazor();

appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEntityFramework(efOptionsBuilder =>
    {
        efOptionsBuilder.UseDbContext<MyDbContext>();
    });
    ...
});
```

Make your application to use the admin panel:

```csharp
app.UseNetForge();
```

## Running the admin panel

By default, NetForge is running on /netforge but you can configure to use your custom endpoint like this:
```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEndpoint("/admin");
    ...
});
```
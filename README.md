# NetForge - Admin Panel for ASP.NET Core 6 & 7

The NetForge is a library that provides a user-friendly and intuitive user interface for performing CRUD operations on your database entities within your .NET 6 and 7 applications.

## How to use

Add NetForge to your service collection in Program.cs:

```csharp
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

## Customizing the endpoint

By default, NetForge Admin is running on */admin* but you can configure to use your custom endpoint like this:

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEndpoint("/manage");
    ...
});
```

## Customizing entities

In the admin panel, you can customize the way entities are displayed using the Fluent API or special attribites. This enables you to set various properties for your entities, such as their name, description, plural name, etc.

### Fluent API

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
     optionsBuilder.UseEntityFramework(efOptionsBuilder =>
     {
         efOptionsBuilder.UseDbContext<MyDbContext>();
     });

     // Set the description to display for the Shop entity.
     optionsBuilder.ConfigureEntity<Shop>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetDescription("Represents the shop.");
     });

     // Hide ProductTag from the admin panel.
     optionsBuilder.ConfigureEntity<ProductTag>(entityOptionsBuilder =>
     {
         entityOptionsBuilder.SetIsHidden(true);
     });
});
```

### Creating an Entity Configuration Class

To reduce the amount of the code all configuration for an entity type can also be extracted to a separate class.

To create an entity configuration for a specific entity type, create a new class that implements the `IEntityAdminConfiguration<TEntity>` interface, where `TEntity` is the type of the entity you want to configure. For example, if you want to configure the `Product` entity, your class might look like this:

```csharp
public class ProductAdminConfiguration : IEntityAdminConfiguration<Product>
{
    public void Configure(EntityOptionsBuilder<Product> builder)
    {
        // Define entity-specific settings here.
    }
}

// Add this to your Program.cs.
appBuilder.Services.AddNetForge(optionsBuilder =>
{
     optionsBuilder.ConfigureEntity(new ProductAdminConfiguration());

     // Other settings...
});
```

### Attributes

You can also customize your entities by applying special attributes directly to your entity classes.

#### NetForgeEntityAttribute

```csharp
[NetForgeEntity(DisplayName = "Entity", PluralName = "Entities", Description = "This is an entity description.")]
public class Entity
{
    // Entity properties...
}
```

You can use the built-in `System.ComponentModel.DescriptionAttribute` and `System.ComponentModel.DisplayNameAttribute` to specify descriptions and display names for your entity classes.

For example:

```csharp
[Description("Custom entity description.")]
[DisplayName("Custom Entity Display Name")]
public class AnotherEntity
{
    // Entity properties...
}
```

## Customizing entity properties

You can customize entity properties as well. For example, you can change display name, add description, hide it or change property column order.

### Fluent API

```csharp
optionsBuilder.ConfigureEntity<Address>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(address => address.Id, propertyBuilder =>
    {
        propertyBuilder
            .SetDescription("Item identifier.")
            .SetOrder(2);
    });

    entityOptionsBuilder.ConfigureProperty(address => address.ContactPhone, propertyBuilder =>
    {
        propertyBuilder
            .SetDisplayName("Phone")
            .SetDescription("Address contact phone.")
            .SetOrder(1);
    });

    entityOptionsBuilder.ConfigureProperty(address => address.PostalCode, propertyBuilder =>
    {
        propertyBuilder.SetIsHidden(true);
    });

    entityOptionsBuilder.ConfigureProperty(address => address.City, propertyBuilder =>
    {
        propertyBuilder.SetDisplayName("Town");
    });
});
```

### Attributes

Properties also customizable via attributes.

#### NetForgeEntityAttribute

```csharp
[NetForgeProperty(DisplayName = "Custom property display name", Description = "Custom property description.", Order = 5)]
public string Property { get; set; }
```
#### Built in `Description` and `DisplayName` attributes

```csharp
[Description("Custom property description.")]
[DisplayName("Custom property display name")]
public string Property { get; set; }
```

## Display formatting

You can configure the display format for the properties values. See [string.Format](https://learn.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting#format-string-component).

### Using Data Attributes

You can apply the `[NetForgeProperty]` attribute to an entity property and specify the display format:

```csharp
[NetForgeProperty(DisplayFormat = "{0:C}")]
public decimal Price { get; set; }
```

In this example, the Price property will be displayed using the currency format.

### Using Fluent API

Alternatively, you can use the Fluent API to configure the display format and format provider for an entity property:

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.ConfigureEntity<Product>(entityOptionsBuilder =>
    {
        entityOptionsBuilder.ConfigureProperty(product => product.Price, propertyBuilder =>
        {
            propertyBuilder.SetDisplayFormat("{0:C}");

            // Use Euro as a currency.
            propertyBuilder.SetFormatProvider(CultureInfo.GetCultureInfo("fr-FR"));
        });
    });

    // Other settings...
});
```

## Search

You can read about search [here](docs/SEARCH.md)

## Grouping

You can read about grouping [here](docs/Grouping.md)
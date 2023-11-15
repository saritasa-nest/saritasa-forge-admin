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

## Customizing the header title and HTML title

You can customize the header title like this:

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.SetHeaderTitle("title");
    ...
});
```

And customize the HTML title like this:

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.SetHtmlTitle("title");
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

Group rows of entities into categories and make it easier for users to navigate and understand the data presented.

### Create Groups for Entities

Before assigning entities to specific groups, users need to define the groups to which the entities will belong.
To create a new group, utilize the Fluent API through `AdminOptionsBuilder`. A name is required for each group, and a description is optional.

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEntityFramework(efOptionsBuilder =>
    {
        efOptionsBuilder.UseDbContext<ShopDbContext>();
    }).AddGroups(new List<EntityGroup>
    {
        new EntityGroup{ Name = "Product", Description = "Contains all information related to products" },
        new EntityGroup{ Name = "Shop"}
    }).ConfigureEntity<Shop>(entityOptionsBuilder =>
    {
        entityOptionsBuilder.SetDescription("The base Shop entity.");
    });
});
```

### Configuration

By default, entities are assigned to the "empty" group. Grouping can be customized either through the Fluent API or by using attributes.
When assigning entities to a group, users only need to specify the group's name. If user specifies a group that does not exists for an entity,
that entity will belong to the default group.

### Fluent API

By utilizing `EntityOptionsBuilder`, user can set group for entity using group's name.

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.UseEntityFramework(efOptionsBuilder =>
    {
        efOptionsBuilder.UseDbContext<ShopDbContext>();
    }).AddGroups(new List<EntityGroup>
    {
        new EntityGroup{ Name = "Product", Description = "Contains all information related to products" },
        new EntityGroup{ Name = "Shop"}
    }).ConfigureEntity<Shop>(entityOptionsBuilder =>
    {
        entityOptionsBuilder.SetGroup("Shop");
        entityOptionsBuilder.SetDescription("The base Shop entity.");
    });
});
```

### Attribute

```csharp
[NetForgeEntity(GroupName = "Product")]
public class ProductTag
```

## Calculated Properties
Calculated properties are properties that don't have a direct representation in your database but are computed based on other existing properties. These properties can be useful for displaying calculated values in the admin panel.

You can add calculated properties to your entities using the Fluent API:

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
    {
        entityOptionsBuilder.AddCalculatedProperties(user => user.FullName, user => user.Age);
    });

    // Other settings...
});
```

## Data sorting

You can apply alphabet sorting to some properties. By default, they are not sortable.

It is configurable via `[NetForgeProperty]` and `Fluent API`.

### Using Attribute

```csharp
[NetForgeProperty(IsSortable = true)]
public string Name { get; set; }
```

### Using Fluent API

```csharp
entityOptionsBuilder.ConfigureProperty(shop => shop.OpenedDate, builder =>
{
    builder.SetIsSortable(true);
});
```

You can sort multiple properties at once. It can be achieved by pressing sort buttons with `CTRL`.

Sorting can be cancelled by pressing on it with `ALT`.

## Navigation properties

You can read about search [here](docs/NAVIGATIONS.md)
# NetForge - Admin Panel for ASP.NET Core 6 & 7 & 8

The NetForge is a library that provides a user-friendly and intuitive user interface for performing CRUD operations on your database entities within your .NET applications.

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

## Customizing access policies for the admin panel

You can customize the access policy by requiring specific roles:

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.AddAccessRoles("Role1", "Role2", "Role3");
    ...
});
```

Alternatively, you can use a custom function to perform checks. Access the required service through the `serviceProvider` parameter. Example:

```csharp
appBuilder.Services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.ConfigureAuth(serviceProvider =>
    {
        // Allow all authenticated users to see the Admin Panel.
        var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        return Task.FromResult(httpContext?.User.Identity?.IsAuthenticated ?? false);
    });
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

### Groups Headers Expanded

You can customize expanded header of all groups. By default, all groups are expanded.

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.SetGroupHeadersExpanded(true)
});
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

## View Site URL

Located in the top right corner of the admin panel is a "View Site" link, configurable to direct users to the website URL.
The default URL is "/".You can customize this value using the Fluent API:

### Using Fluent API

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.ConfigureUrl("https://www.example.com/");
});
```

## Display Properties as Title Case

By default, all entity properties are displayed in Title Case.

For example, the `Product` entity will have the property `StockQuantity`. By default, it will be displayed as `Stock Quantity` in the admin panel.
This behavior can be disabled, and the entities will use CamelCase display instead.

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.DisableTitleCaseProperties();
});
```

## Set the default value for empty property records.

Users can customize the value used for displaying the empty record values. By default, it will be displayed as "-" (a dash).

### Using Fluent API

```csharp
optionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(user => user.DateOfBirth,
        propertyBuilder => propertyBuilder.SetEmptyValueDisplay("N/A"));
});
```

### Using Attribute

```csharp
[NetForgeProperty(EmptyValueDisplay = "N/A")]
public string Property { get; set; }
```

## Navigation properties

You can read about navigation properties [here](docs/NAVIGATIONS.md)

# Custom query

You can configure your query for specific entity.

```csharp
.ConfigureEntity<Shop>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureCustomQuery((serviceProvider, query) =>
    {
        return query.Where(e => e.IsOpen == true);
    });
})
```

You can use `ServiceProvider` to access your services.

## Exclude property from query

You can explicitly control whether a property should be excluded from the data query.

### Using Fluent API

```csharp
optionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(user => user.DateOfBirth,
        propertyBuilder => propertyBuilder.SetIsExcludedFromQuery(true));
});
```

### Using Attribute

```csharp
[NetForgeProperty(IsExcludeFromQuery = true)]
public string Property { get; set; }
```

## Formatting property as HTML

You can configure certain entity properties to be rendered as HTML content in the data grid.

### Using Fluent API

```csharp
optionsBuilder.ConfigureEntity<User>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(user => user.Id,
        propertyBuilder => propertyBuilder.SetDisplayAsHtml(true));
});
```

### Using Attribute

```csharp
[NetForgeProperty(DisplayAsHtml = true)]
public string Property { get; set; }
```

## Generated Properties

Generated properties will not be displayed on the create or edit entity pages. Entity framework example of generated property:

```csharp
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public DateTime CreatedAt { get; set; }
```

## Rich Text Field

RTF provides some common text formatting options like paragraphs, links, tables, etc.
The [ClassicEditor of CKEditor 5](https://ckeditor.com/docs/ckeditor5/latest/examples/builds/classic-editor.html) is used by the admin panel.

The configuration is the following:

### Using Attribute
```csharp
[NetForgeProperty(IsRichTextField = true)]
public required string Description { get; set; }
```

### Using Fluent API
```csharp
optionsBuilder.ConfigureEntity<Product>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(product => product.Description,
        propertyBuilder => propertyBuilder.SetIsRichTextField(true));
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
```

# Image Properties

You can add properties that will be displayed as images. They can be configured only via `Fluent API`.

## Max image size

You can set max image size in the application. Default value for max image size is 10 MB.

``` csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.SetMaxImageSize(15);
});
```

## Configuration

### Using Fluent API

You can create your own implementaion of `IUploadFileStrategy` interface and pass it to `SetUploadFileStrategy` method.

This interface has methods `UploadFileAsync` that is calling when file is uploaded and `GetFileSource` that is calling when file should be displayed.

We have some examples of strategies [here](demo/Saritasa.NetForge.Demo/Infrastructure/UploadFiles/Strategies).

```csharp
entityOptionsBuilder.ConfigureProperty(shop => shop.BuildingPhoto, builder =>
{
    builder
        .SetIsImage(true)
        .SetUploadFileStrategy(new UploadBase64FileStrategy());
});
```

# Read only property

You can mark a property as read only. Such property cannot be changed on create and edit pages.

## Configuration

### Using Fluent API

```csharp
entityOptionsBuilder.ConfigureProperty(product => product.UpdatedDate, builder =>
{
    builder.SetIsReadOnly(true);
});
```

### Using Attribute

```csharp
[NetForgeProperty(IsReadOnly = true)]
public string Property { get; set; }
```

# String Truncate

You can set the max characters amount for string properties.

## Configuration

### Global

You can set max characters for the all strings. Default value is 50.

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.SetTruncationMaxCharacters(60);
});
```

You can disable this behavior in this way:

```csharp
services.AddNetForge(optionsBuilder =>
{
    optionsBuilder.DisableCharactersTruncation();
});
```

### For the Property

You can set max characters to each property individually. Disabled by default because property does not have default value.

#### Using Attribute

```csharp
[NetForgeProperty(TruncationMaxCharacters = 20)]
public string Name { get; set; }
```

#### Using Fluent API

```csharp
entityOptionsBuilder.ConfigureProperty(shop => shop.Name, builder =>
{
    builder.SetTruncationMaxCharacters(25);
});
```
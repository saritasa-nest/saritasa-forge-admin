# NetForge - Navigations

By default, navigation properties are hidden from view. But you can customize it for each entity.

## Relationships

Relationships are displayed in the following ways:

### Reference

When navigation represents a reference to another entity, the primary key of this entity will be displayed.

### Collection

When navigation represents by a collection, the primary keys of all items will be displayed. For example: `[1, 15, 99]`.

## Display navigations configuration

Displaying navigation properties are configurable via `[NetForgeNavigation]` and `Fluent API`.

### Using attribute

```csharp
[NetForgeNavigation(IsIncluded = true)]
public List<ProductTag> Tags { get; set; } = new();
```

### Using Fluent API

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder
        .IncludeNavigations(
            shop => shop.Address,
            shop => shop.OwnerContact,
            shop => shop.Products,
            shop => shop.Suppliers);
}
```

## Navigation property configuration

Navigation property can be configured in similar way with property.

When configuring navigation property there are some property configuration feature that are not appliable to navigation:

    1. SearchType
    2. IsSortable

### Using attribute

Navigations uses different attribute - `NetForgeNavigation`.

```csharp
[NetForgeNavigation(DisplayName = "AddressId", Description = "Address identifier.", Order = 1)]
public Address? Address { get; set; }
```

### Using Fluent API

Navigation Fluent API configuration uses the same method as property - `ConfigureProperty`.

```csharp
entityOptionsBuilder.ConfigureProperty(shop => shop.OwnerContact, builder =>
{
    builder
        .SetDisplayName("OwnerContactInfo")
        .SetDescription("Information about owner contact.")
        .SetOrder(2);
});
```
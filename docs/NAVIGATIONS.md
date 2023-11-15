# NetForge - Navigations

By default, navigation properties are hidden from view. But you can customize it for each entity.

## Relationships

Relationships are displayed in that way:

### Reference

When navigation represents reference to another entity, then primary key of this entity will be displayed.

### Collection

When navigation represents collection, then primary keys of all items will be displayed. For example: `[1, 15, 99]`

## Display navigations configuration

Displaying navigation properties are configurable via `[NetForgeProperty]` and `Fluent API`.

### Using attribute

```csharp
[NetForgeEntity(IsDisplayNavigations = true)]
public class Product
```

### Using Fluent API

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder.SetIsDisplayNavigations(true)
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

Navigation Fluent API configuration uses different method - `ConfigureNavigation`.

```csharp
entityOptionsBuilder.ConfigureNavigation(shop => shop.OwnerContact, builder =>
{
    builder
        .SetDisplayName("OwnerContactData")
        .SetDescription("Information about owner contact.")
        .SetOrder(2);
});
```
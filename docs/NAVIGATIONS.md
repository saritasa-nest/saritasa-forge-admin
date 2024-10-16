# NetForge - Navigations

By default, navigation properties are hidden from view. But you can customize it for each entity.

## Relationships

Relationships are displayed in the following ways:

### Reference

When navigation represents a reference to another entity, then you can choose which navigation's property to display.

### Collection

In case of the collection navigation we support displaying only primary keys. For example: `[1, 15, 99]`.

## Display navigations configuration

Displaying navigation properties are configurable via `Fluent API`.

### Using Fluent API

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder
        .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder
                .IncludeProperty(address => address.Id, builder =>
                {
                    builder
                        .SetOrder(3)
                        .SetDisplayName("Address Id");
                })
                .IncludeProperty(address => address.Street, builder =>
                {
                    builder
                        .SetOrder(4)
                        .SetDisplayName("Address Street")
                        .SetDescription("Address street name.")
                        .SetEmptyValueDisplay("N/A")
                        .SetIsSortable(true)
                        .SetSearchType(SearchType.ContainsCaseInsensitive);
                });
        });
}
```

## Displaying navigations

To display friendly name of a navigation on create/edit entity pages you need to override `ToString`.

```csharp
public override string ToString()
{
    return $"{Id}; {Name}";
}
```

## Display navigation data on the entity List View page

You can click on the navigation on the entity details to see navigation details.

### Reference

For reference navigation you need to configure it via `Fluent API` using `SetShowNavigationDetails` method.
When you use this method to a navigation's property, then this property will be rendered as button.
This method accepts `isReadonly` parameter, when it is `true`, then by pressing the button dialog with navigation details appears.
Otherwise, when it is `false` you will be redirected to edit corresponding entity page.

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder
            .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
            {
                navigationOptionsBuilder
                    .IncludeProperty(address => address.Street, builder =>
                    {
                        builder.SetShowNavigationDetails(isReadonly: false);
                    })
                    .IncludeProperty(address => address.City, builder =>
                    {
                        builder.SetShowNavigationDetails(isReadonly: true);
                    });
            })
}
```

### Collection

Collection navigations have this behavior by default like it was configured using `SetShowNavigationDetails(isReadonly: true)`.
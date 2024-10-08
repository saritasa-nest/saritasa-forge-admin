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

## Display navigation data on the entity details page

You can click on the navigation on the entity details to see navigation data in dialog.

### Reference

For reference navigation you need to configure it via `Fluent API`. Note that only primary key of the entity is clickable to see navigation data.

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder
        .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder
                .IncludeProperty(address => address.Id, builder =>
                {
                    builder.SetDisplayName("Address Id");
                })
                .SetIsDisplayDetails(true);
        })
}
```

### Collection

Collection navigations have this behavior by default.

## Edit navigation data

You can edit navigation data directly from parent entity list view page by click on the navigation link.

### Reference

For reference edit of navigation data you need to configure it via `Fluent API`. Note that only primary key of the entity is clickable to edit navigation data.
Note that if both navigation methods are called, the .SetAsEditable() method has higher priority than .SetIsDisplayDetails(true).

```csharp
public void Configure(EntityOptionsBuilder<Shop> entityOptionsBuilder)
{
    entityOptionsBuilder
        .IncludeNavigation<Address>(shop => shop.Address, navigationOptionsBuilder =>
        {
            navigationOptionsBuilder
                .IncludeProperty(address => address.Id, builder =>
                {
                    builder.SetDisplayName("Address Id");
                })
                .SetIsDisplayDetails(true)
				.SetAsEditable();
        })
}
```
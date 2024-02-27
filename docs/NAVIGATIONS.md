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
                .SetOrder(1)
                .IncludeProperty(address => address.Id, builder =>
                {
                    builder.SetDisplayName("Address Id");
                })
                .IncludeProperty(address => address.Street, builder =>
                {
                    builder
                        .SetDisplayName("Address Street")
                        .SetDescription("Address street name.")
                        .SetEmptyValueDisplay("N/A")
                        .SetIsSortable(true)
                        .SetSearchType(SearchType.ContainsCaseInsensitive);
                });
        });
}
```

## Requirements to navigation entities

### ToString

To display friendly name of a navigation on create/edit entity pages override `ToString`.

```csharp
public override string ToString()
{
    return $"{Id}; {Name}";
}
```

### Equals and GetHashCode

We use <MudSelect> from `MudBlazor` library to display navigations, and there are requirement when not primitive type was used.
To compare custom types between each other you need to override `Equals` and `GetHashCode` methods.

For example:

```csharp
public override bool Equals(object? obj) 
{
    return obj?.ToString() == ToString();
}

public override int GetHashCode()
{
    return ToString().GetHashCode();
}
```
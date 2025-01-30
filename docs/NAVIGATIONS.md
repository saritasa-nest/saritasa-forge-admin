# NetForge - Navigations

By default, navigation properties are hidden from view. But you can customize it for each entity.

## Relationships

Relationships are displayed in the following ways:

### Reference

When navigation represents a reference to another entity, then you can choose which navigation's property to display.

### Collection

In case of the collection navigation we support displaying only primary keys. For example: `[1, 15, 99]`.

## Include Navigation

You can include navigation and its properties via `Fluent API`.

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

## Display navigation data on the entity Edit page

You can set function to get string representation of an entity.

### Using Fluent API

```csharp
    public void Configure(EntityOptionsBuilder<Address> entityOptionsBuilder)
    {
        entityOptionsBuilder.ConfigureToString(address => $"{address.Street}, {address.City}");
    }
```

### ToString Override

Also, if you have `ToString` overridden then it will be used to get string representation. 
But `ConfigureToString` has precedence.

### Primary Keys Display

If, neither `ToString` and `ConfigureToString` are implemented, then primary keys will be displayed.
In case of composite key `--` will be used as separator. For example:

`Tokyo--Japan`

### Equals and GetHashCode Override

To display currently selected item in select input when navigation reference is used you have to override `Equals` and `GetHashCode`.
It is restriction of `MudBlazor` library. See code of `Custom converter` section [here](https://www.mudblazor.com/components/select).
It is not working for displaying navigation collection.

```csharp
    public override bool Equals(object? obj)
    {
        if (obj is not Address address)
        {
            return false;
        }

        return address.Street == Street && address.City == City;
    }

    public override int GetHashCode() => Street.GetHashCode() + City.GetHashCode();
```
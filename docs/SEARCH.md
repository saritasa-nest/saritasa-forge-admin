# NetForge - Search

## Search input on entity details page

Every word in the input will become separate search entry. Phrases between quoutes: `"` or `'` will also become separate entries.
For example take a look at this search string: `"Double quotes" 'Single quotes' Without quotes`.
There are these search entries:

    1. Double quotes
    2. Single quotes
    3. Without
    4. quotes

## Search types

Every property can have their own search type.

Now the project have these search types:

    * Starts With Case Sensitive;
    * Exact Match Case Insensitive;
    * Contains Case Insensitive.

Every searchable property will be searched by every search entry.

For example:

```SQL
SELECT ... FROM ...
WHERE (Name = searchEntry1 OR Description LIKE searchEntry1 OR ...)
AND (Name = searchEntry2 OR Description LIKE searchEntry2 OR ...)
AND (Name = searchEntry3 OR Description LIKE searchEntry3 OR ...)
AND (...)
```

By default properties do not have search types.
Search type can be customized via Fluent API or attribute.

### Fluent API

```csharp
.ConfigureEntity<Product>(entityOptionsBuilder =>
{
    entityOptionsBuilder.ConfigureProperty(product => product.Name, builder =>
    {
        builder.SetSearchType(SearchType.ExactMatchCaseInsensitive);
    });
});
```

### Attribute

```csharp
[NetForgeProperty(SearchType = SearchType.StartsWithCaseSensitive)]
public string Name { get; set; }
```
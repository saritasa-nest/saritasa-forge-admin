# NetForge - Search

## Search input on entity details page

Every word in the input will become separate search entry. Phrases between quotes: `"` or `'` will also become separate entries.
For example take a look at this search string: `"Double quotes" 'Single quotes' Without quotes`.
There are these search entries:

    1. Double quotes
    2. Single quotes
    3. Without
    4. quotes

## Search types

Every property can have their own search type.

Search types will generate SQL similar to:

### Starts With Case Sensitive

```SQL
SELECT ... WHERE name LIKE 'Lennon%';
```

### Exact Match Case Insensitive


```SQL
SELECT ... WHERE name ILIKE 'beatles blog';
```

If the value provided for comparison is `None`, it will be interpreted as a SQL `NULL`.

```SQL
SELECT ... WHERE name IS NULL;
```

### Contains Case Insensitive

```SQL
SELECT ... WHERE name ILIKE '%Lennon%';
```

Every searchable property will be searched by every search entry.

For example:

```SQL
SELECT ... FROM ...
WHERE (Name LIKE searchEntry1 OR Description ILIKE searchEntry1 OR ...)
AND (Name LIKE searchEntry2 OR Description ILIKE searchEntry2 OR ...)
AND (Name LIKE searchEntry3 OR Description ILIKE searchEntry3 OR ...)
AND (...)
```

## Configuration

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

# Custom Search

You can configure your own search.

```csharp
.ConfigureEntity<Shop>(entityOptionsBuilder =>
    {
        entityOptionsBuilder
            .ConfigureSearch((serviceProvider, query, searchTerm) =>
            {
                return query.Where(e => e.Name.Contains(searchTerm));
            });
    })
```

You can use `ServiceProvider` to access your services.
using System.Collections;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <summary>
/// Convert entities with lazy loading proxies to POCO entities.
/// </summary>
public static class ProxyToPocoConverter
{
    /// <summary>
    /// Converts a proxy entity to a POCO entity.
    /// </summary>
    /// <param name="source">The source proxy entity.</param>
    /// <param name="navigationPropertyNames">The list of navigation property names.</param>
    /// <returns>The POCO entity.</returns>
    public static object? ConvertProxyToPoco(object? source, IList<string>? navigationPropertyNames)
    {
        switch (source)
        {
            case null:
                return default;

            // Sometimes the source is a collection of proxy entities.
            case IEnumerable<object> sourceCollection:
                {
                    var sourceType = source.GetType();

                    // Get the item type of the collection to avoid boxing issues.
                    var itemType = sourceType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                    var pocoCollectionType = typeof(List<>).MakeGenericType(itemType);
                    var pocoCollection = (IList)Activator.CreateInstance(pocoCollectionType)!;

                    foreach (var item in sourceCollection)
                    {
                        pocoCollection.Add(ConvertProxyToPoco(item, null));
                    }
                    return pocoCollection;
                }
        }

        var entityType = source.GetPocoType();

        // Create new instance of the POCO entity.
        var pocoInstance = Activator.CreateInstance(entityType)!;

        // Copy the properties from the proxy to the POCO.
        foreach (var property in entityType.GetProperties())
        {
            // Exclude the navigation properties because they are the proxies as well.
            if (navigationPropertyNames != null && navigationPropertyNames.Contains(property.Name))
            {
                continue;
            }

            // Exclude indexers, read-only and write-only properties.
            if (!property.CanWrite || !property.CanRead || property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            try
            {
                var value = property.GetValue(source);

                // Skip the property if it is a proxy.
                if (value != null && value.GetType().IsLazyLoadingProxy())
                {
                    continue;
                }

                // Check if the property is a collection of proxy entities.
                if (value is IEnumerable<object> collection)
                {
                    var firstItem = collection.FirstOrDefault();
                    if (firstItem != null && firstItem.GetType().IsLazyLoadingProxy())
                    {
                        continue;
                    }
                }

                property.SetValue(pocoInstance, value);
            }
            catch
            {
                // Skip the property if it cannot be copied.
            }
        }

        if (navigationPropertyNames == null)
        {
            return pocoInstance;
        }

        // Do the same for the navigation properties recursively because they can be proxies as well.
        foreach (var navigationName in navigationPropertyNames)
        {
            var property = source.GetType().GetProperty(navigationName);
            var propertyValue = property?.GetValue(source);

            if (property == null)
            {
                continue;
            }

            var navigationPoco = ConvertProxyToPoco(propertyValue, null);
            entityType.GetProperty(navigationName)?.SetValue(pocoInstance, navigationPoco);
        }

        return pocoInstance;
    }
}

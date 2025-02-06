using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

namespace Saritasa.NetForge.Infrastructure.EfCore.Services;

/// <summary>
/// Convert entities with lazy loading proxies to POCO entities.
/// </summary>
public class ProxyToPocoConverter
{
    /// <summary>
    /// Converts a proxy entity to a POCO entity.
    /// </summary>
    /// <param name="source">The source proxy entity.</param>
    /// <param name="navigationPropertyNames">The list of navigation property names.</param>
    /// <returns>The POCO entity.</returns>
    public object? ConvertProxyToPoco(object? source, IList<string> navigationPropertyNames)
    {
        if (source == null)
        {
            return default;
        }

        var entityType = source.GetPocoType();

        // Create new instance of the POCO entity.
        var pocoInstance = Activator.CreateInstance(entityType)!;

        // Copy the properties from the proxy to the POCO.
        foreach (var property in entityType.GetProperties())
        {
            // Exclude the navigation properties because they are the proxies as well.
            // Exclude indexers, read-only and write-only properties.
            if (navigationPropertyNames.Contains(property.Name) || !property.CanWrite || !property.CanRead
                || property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            try
            {
                var value = property.GetValue(source);
                property.SetValue(pocoInstance, value);
            }
            catch
            {
                // Skip the property if it cannot be copied.
            }
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

            var navigationPoco = ConvertProxyToPoco(propertyValue, []);
            entityType.GetProperty(navigationName)?.SetValue(pocoInstance, navigationPoco);
        }

        return pocoInstance;
    }
}

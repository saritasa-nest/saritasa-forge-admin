using System.Reflection;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Infrastructure.Helpers;

/// <summary>
/// Extensions for getting data from entity instances.
/// </summary>
public static class EntityInstanceExtensions
{
    /// <summary>
    /// Gets primary key values from given <paramref name="instance"/>.
    /// </summary>
    /// <param name="properties">Metadata of properties that <paramref name="instance"/> have.</param>
    /// <param name="instance">Entity instance.</param>
    /// <returns>Primary key values as string separated by --, e.g. <c>111--New-York--Company INC.</c></returns>
    public static string GetPrimaryKeyValues(this object instance, IEnumerable<PropertyMetadataDto> properties)
    {
        var primaryKeyValues = properties
            .Where(property => property.IsPrimaryKey)
            .Select(primaryKey => instance.GetPropertyValue(primaryKey.Name)!.ToString()!);
        return string.Join("--", primaryKeyValues);
    }

    /// <summary>
    /// Uses properties with <see cref="NetForgePropertyAttribute.UseToDisplayNavigation"/>
    /// to convert an instance to string.
    /// </summary>
    /// <param name="instance">Instance to convert.</param>
    /// <returns>
    /// String representation of given instance. All suitable properties will be separated by <c>;</c>.
    /// For example: Main Street; New-York; United States.
    /// </returns>
    public static string ConvertToString(this object? instance)
    {
        if (instance is null)
        {
            return string.Empty;
        }

        if (HasToStringOverride(instance))
        {
            return instance.ToString()!;
        }

        var propertiesToDisplay = instance
            .GetType()
            .GetProperties()
            .Select(property => (PropertyInfo: property, Attribute: property.GetCustomAttribute<NetForgePropertyAttribute>()))
            .Where(property => property.Attribute is not null && property.Attribute.UseToDisplayNavigation);

        List<object> propertyValues = [];
        foreach (var property in propertiesToDisplay)
        {
            var propertyValue = property.PropertyInfo.GetValue(instance);
            propertyValues.Add(propertyValue ?? property.Attribute!.EmptyValueDisplay);
        }

        return string.Join("; ", propertyValues);
    }

    private static bool HasToStringOverride(object obj)
    {
        var type = obj.GetType();
        var toStringMethod = type.GetMethod(nameof(ToString))!;

        // If DeclaringType is not object then the method is overridden.
        return toStringMethod.DeclaringType != typeof(object);
    }
}

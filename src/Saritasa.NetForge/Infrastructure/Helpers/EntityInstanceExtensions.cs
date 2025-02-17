using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Infrastructure.Helpers;

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
    /// Gets string representation of given instance.
    /// </summary>
    /// <param name="instance">Instance to convert.</param>
    /// <param name="entityMetadata">Entity metadata.</param>
    /// <returns>
    /// String representation of given instance.
    /// <list type="number">
    ///     <item>
    ///         If <paramref name="entityMetadata"/> has <see cref="GetEntityDto.ToStringFunc"/>, then it will be used.
    ///     </item>
    ///     <item>
    ///         If <see cref="GetEntityDto.ToStringFunc"/> is <c>null</c>,
    ///         then <see cref="object.ToString()"/> override will be used.
    ///     </item>
    ///     <item>
    ///         If <see cref="GetEntityDto.ToStringFunc"/> is <c>null</c>
    ///         and entity does not have <see cref="object.ToString()"/> override,
    ///         then primary keys of an entity will be used.
    ///     </item>
    /// </list>
    /// </returns>
    public static string ConvertToString(this object? instance, GetEntityDto entityMetadata)
    {
        if (instance is null)
        {
            return string.Empty;
        }

        if (entityMetadata.ToStringFunc is not null)
        {
            return entityMetadata.ToStringFunc.Invoke(instance);
        }

        if (HasToStringOverride(instance))
        {
            return instance.ToString()!;
        }

        return instance.GetPrimaryKeyValues(entityMetadata.Properties);
    }

    private static bool HasToStringOverride(object obj)
    {
        var type = obj.GetType();
        var toStringMethod = type.GetMethod(nameof(ToString))!;

        // If DeclaringType is not object then the method is overridden.
        return toStringMethod.DeclaringType != typeof(object);
    }
}

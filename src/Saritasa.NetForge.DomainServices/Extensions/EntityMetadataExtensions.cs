using System.ComponentModel;
using System.Reflection;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// Provides extension methods to apply entity-specific options and attributes to <see cref="EntityMetadata"/>.
/// </summary>
public static class EntityMetadataExtensions
{
    /// <summary>
    /// Applies entity-specific options to the given <see cref="EntityMetadata"/> using the provided options.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which options are applied.</param>
    /// <param name="entityOptions">Options to apply for the entity metadata.</param>
    public static void ApplyOptions(this EntityMetadata entityMetadata, EntityOptions entityOptions)
    {
        if (!string.IsNullOrEmpty(entityOptions.Description))
        {
            entityMetadata.Description = entityOptions.Description;
        }

        if (!string.IsNullOrEmpty(entityOptions.DisplayName))
        {
            entityMetadata.DisplayName = entityOptions.DisplayName;
        }

        if (!string.IsNullOrEmpty(entityOptions.PluralName))
        {
            entityMetadata.PluralName = entityOptions.PluralName;
        }

        if (entityOptions.IsHidden)
        {
            entityMetadata.IsHidden = entityOptions.IsHidden;
        }

        if (entityOptions.SearchFunction is not null)
        {
            entityMetadata.SearchFunction = entityOptions.SearchFunction;
        }

        foreach (var option in entityOptions.PropertyOptions)
        {
            var property = entityMetadata.Properties.FirstOrDefault(property => property.Name == option.PropertyName);

            property?.ApplyPropertyOptions(option);
        }

        if (!string.IsNullOrEmpty(entityOptions.GroupName))
        {
            var entityGroup =
                adminOptions.EntityGroupsList.FirstOrDefault(group => group.Name == entityOptions.GroupName);
            if (entityGroup != null)
            {
                entityMetadata.Group = entityGroup;
            }
        }
    }

    private static void ApplyPropertyOptions(
        this PropertyMetadata property, PropertyOptions propertyOptions)
    {
        property.IsHidden = propertyOptions.IsHidden;

        if (!string.IsNullOrEmpty(propertyOptions.DisplayName))
        {
            property.DisplayName = propertyOptions.DisplayName;
        }

        if (!string.IsNullOrEmpty(propertyOptions.Description))
        {
            property.Description = propertyOptions.Description;
        }

        if (propertyOptions.Order.HasValue)
        {
            property.Order = propertyOptions.Order.Value;
        }

        property.DisplayFormat = propertyOptions.DisplayFormat ?? property.DisplayFormat;
        property.FormatProvider = propertyOptions.FormatProvider ?? property.FormatProvider;

        property.SearchType = propertyOptions.SearchType;
    }

    /// <summary>
    /// Applies entity-specific attributes to the given <see cref="EntityMetadata"/>.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which attributes are applied.</param>
    /// <param name="adminOptions">Options to apply for the entity metadata.</param>
    public static void ApplyEntityAttributes(this EntityMetadata entityMetadata, AdminOptions adminOptions)
    {
        foreach (var property in entityMetadata.Properties)
        {
            property.ApplyPropertyAttributes();
        }

        // Try to get the description from the System.ComponentModel.DisplayNameAttribute.
        var displayNameAttribute = entityMetadata.ClrType?.GetCustomAttribute<DisplayNameAttribute>();

        if (!string.IsNullOrEmpty(displayNameAttribute?.DisplayName))
        {
            entityMetadata.DisplayName = displayNameAttribute.DisplayName;
        }

        var descriptionAttribute = entityMetadata.ClrType?.GetCustomAttribute<DescriptionAttribute>();

        if (!string.IsNullOrEmpty(descriptionAttribute?.Description))
        {
            entityMetadata.Description = descriptionAttribute.Description;
        }

        var netForgeEntityAttribute = entityMetadata.ClrType?.GetCustomAttribute<NetForgeEntityAttribute>();

        if (netForgeEntityAttribute == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.Description))
        {
            entityMetadata.Description = netForgeEntityAttribute.Description;
        }

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.DisplayName))
        {
            entityMetadata.DisplayName = netForgeEntityAttribute.DisplayName;
        }

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.PluralName))
        {
            entityMetadata.PluralName = netForgeEntityAttribute.PluralName;
        }

        if (netForgeEntityAttribute.IsHidden)
        {
            entityMetadata.IsHidden = netForgeEntityAttribute.IsHidden;
        }

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.GroupName))
        {
            var entityGroup =
                adminOptions.EntityGroupsList.FirstOrDefault(e => e.Name == netForgeEntityAttribute.GroupName);
            if (entityGroup != null)
            {
                entityMetadata.Group = entityGroup;
            }
        }
    }

    private static void ApplyPropertyAttributes(this PropertyMetadata property)
    {
        var descriptionAttribute = property.PropertyInformation?.GetCustomAttribute<DescriptionAttribute>();

        if (!string.IsNullOrEmpty(descriptionAttribute?.Description))
        {
            property.Description = descriptionAttribute.Description;
        }

        var displayNameAttribute = property.PropertyInformation?.GetCustomAttribute<DisplayNameAttribute>();

        if (!string.IsNullOrEmpty(displayNameAttribute?.DisplayName))
        {
            property.DisplayName = displayNameAttribute.DisplayName;
        }

        var netForgePropertyAttribute = property.PropertyInformation?.GetCustomAttribute<NetForgePropertyAttribute>();

        if (netForgePropertyAttribute is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(netForgePropertyAttribute.Description))
        {
            property.Description = netForgePropertyAttribute.Description;
        }

        if (!string.IsNullOrEmpty(netForgePropertyAttribute.DisplayName))
        {
            property.DisplayName = netForgePropertyAttribute.DisplayName;
        }

        if (netForgePropertyAttribute.IsHidden)
        {
            property.IsHidden = netForgePropertyAttribute.IsHidden;
        }

        if (netForgePropertyAttribute.Order >= 0)
        {
            property.Order = netForgePropertyAttribute.Order;
        }

        property.DisplayFormat = netForgePropertyAttribute.DisplayFormat ?? property.DisplayFormat;

        if (netForgePropertyAttribute.SearchType != SearchType.None)
        {
            property.SearchType = netForgePropertyAttribute.SearchType;
        }
    }
}

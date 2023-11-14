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
    /// <param name="adminOptions">Options to apply for the entity metadata.</param>
    public static void ApplyOptions(this EntityMetadata entityMetadata, EntityOptions entityOptions, AdminOptions adminOptions)
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

        if (entityOptions.IsDisplayNavigations)
        {
            entityMetadata.IsDisplayNavigations = entityOptions.IsDisplayNavigations;
        }

        foreach (var option in entityOptions.PropertyOptions)
        {
            var property = entityMetadata.Properties.FirstOrDefault(property => property.Name == option.PropertyName);

            property?.ApplyPropertyOptions(option);
        }

        foreach (var option in entityOptions.NavigationOptions)
        {
            var navigation = entityMetadata.Navigations
                .FirstOrDefault(navigation => navigation.Name == option.NavigationName);

            navigation?.ApplyNavigationsOptions(option);
        }

        SetGroupForEntity(entityOptions.GroupName, entityMetadata, adminOptions);
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

        if (propertyOptions.IsSortable)
        {
            property.IsSortable = propertyOptions.IsSortable;
        }
    }

    private static void ApplyNavigationsOptions(
        this NavigationMetadata navigation, NavigationOptions navigationOptions)
    {
        navigation.IsHidden = navigationOptions.IsHidden;

        if (!string.IsNullOrEmpty(navigationOptions.DisplayName))
        {
            navigation.DisplayName = navigationOptions.DisplayName;
        }

        if (!string.IsNullOrEmpty(navigationOptions.Description))
        {
            navigation.Description = navigationOptions.Description;
        }

        if (navigationOptions.Order.HasValue)
        {
            navigation.Order = navigationOptions.Order.Value;
        }

        navigation.DisplayFormat = navigationOptions.DisplayFormat ?? navigation.DisplayFormat;
        navigation.FormatProvider = navigationOptions.FormatProvider ?? navigation.FormatProvider;
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

        foreach (var navigation in entityMetadata.Navigations)
        {
            navigation.ApplyPropertyAttributes();
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

        SetGroupForEntity(netForgeEntityAttribute.GroupName, entityMetadata, adminOptions);

        if (netForgeEntityAttribute.IsDisplayNavigations)
        {
            entityMetadata.IsDisplayNavigations = netForgeEntityAttribute.IsDisplayNavigations;
        }
    }

    private static void ApplyPropertyAttributes(this PropertyMetadataBase property)
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

        var netForgePropertyAttributeBase = property.PropertyInformation?
            .GetCustomAttribute<NetForgePropertyAttributeBase>();

        if (netForgePropertyAttributeBase is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(netForgePropertyAttributeBase.Description))
        {
            property.Description = netForgePropertyAttributeBase.Description;
        }

        if (!string.IsNullOrEmpty(netForgePropertyAttributeBase.DisplayName))
        {
            property.DisplayName = netForgePropertyAttributeBase.DisplayName;
        }

        if (netForgePropertyAttributeBase.IsHidden)
        {
            property.IsHidden = netForgePropertyAttributeBase.IsHidden;
        }

        if (netForgePropertyAttributeBase.Order >= 0)
        {
            property.Order = netForgePropertyAttributeBase.Order;
        }

        property.DisplayFormat = netForgePropertyAttributeBase.DisplayFormat ?? property.DisplayFormat;

        if (property is PropertyMetadata propertyMetadata)
        {
            var netForgePropertyAttribute = (NetForgePropertyAttribute)netForgePropertyAttributeBase;

            if (netForgePropertyAttribute.SearchType != SearchType.None)
            {
                propertyMetadata.SearchType = netForgePropertyAttribute.SearchType;
            }

            if (netForgePropertyAttribute.IsSortable)
            {
                propertyMetadata.IsSortable = netForgePropertyAttribute.IsSortable;
            }
        }
    }

    private static void SetGroupForEntity(string groupName, EntityMetadata entityMetadata, AdminOptions adminOptions)
    {
        if (!string.IsNullOrEmpty(groupName))
        {
            var entityGroup =
                adminOptions.EntityGroupsList.FirstOrDefault(group => group.Name == groupName);
            if (entityGroup != null)
            {
                entityMetadata.Group = entityGroup;
            }
        }
    }
}

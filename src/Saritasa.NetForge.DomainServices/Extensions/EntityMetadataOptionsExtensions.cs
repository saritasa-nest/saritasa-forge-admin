using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.DomainServices.Extensions;

/// <summary>
/// Provides extension methods to apply entity-specific options to <see cref="EntityMetadata"/>.
/// </summary>
public static class EntityMetadataOptionsExtensions
{
    /// <summary>
    /// Applies entity-specific options to the given <see cref="EntityMetadata"/> using the provided options.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which options are applied.</param>
    /// <param name="entityOptions">Options to apply for the entity metadata.</param>
    /// <param name="adminOptions">Admin panel options.</param>
    public static void ApplyOptions(this EntityMetadata entityMetadata, EntityOptions entityOptions,
        AdminOptions adminOptions)
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

        if (entityOptions.CustomQueryFunction is not null)
        {
            entityMetadata.CustomQueryFunction = entityOptions.CustomQueryFunction;
        }

        foreach (var option in entityOptions.PropertyOptions)
        {
            var property = entityMetadata.Properties
                .FirstOrDefault(property => property.Name == option.PropertyName);

            if (property is not null)
            {
                property.ApplyPropertyOptions(option);
                continue;
            }

            var navigation = entityMetadata.Navigations
                .FirstOrDefault(navigation => navigation.Name == option.PropertyName);

            navigation?.ApplyPropertyOptions(option);
        }

        foreach (var navigationOptions in entityOptions.NavigationOptions)
        {
            var navigation = entityMetadata.Navigations
                .First(navigation => navigation.Name.Equals(navigationOptions.PropertyName));

            navigation.ApplyNavigationOptions(navigationOptions);
        }

        entityMetadata.AssignGroupToEntity(entityOptions.GroupName, adminOptions);
    }

    private static void ApplyPropertyOptions(
        this PropertyMetadataBase property, PropertyOptions propertyOptions)
    {
        property.IsHidden = propertyOptions.IsHidden;
        property.IsHiddenFromListView = propertyOptions.IsHiddenFromListView;
        property.IsHiddenFromDetails = propertyOptions.IsHiddenFromDetails;
        property.IsExcludedFromQuery = propertyOptions.IsExcludedFromQuery;

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

        if (!string.IsNullOrEmpty(propertyOptions.EmptyValueDisplay))
        {
            property.EmptyValueDisplay = propertyOptions.EmptyValueDisplay;
        }
    }

    private static void ApplyNavigationOptions(
        this NavigationMetadata navigation, NavigationOptions navigationOptions)
    {
        navigation.IsIncluded = true;

        if (navigationOptions.Order.HasValue)
        {
            navigation.Order = navigationOptions.Order.Value;
        }

        navigation.TargetEntityProperties = navigation.TargetEntityProperties
            .Where(property => navigationOptions.PropertyOptions
                .Any(includedProperty => includedProperty.PropertyName.Equals(property.Name)))
            .ToList();

        foreach (var propertyOptions in navigationOptions.PropertyOptions)
        {
            var property = navigation.TargetEntityProperties
                .FirstOrDefault(property => property.Name == propertyOptions.PropertyName);

            property?.ApplyPropertyOptions(propertyOptions);
        }
    }
}

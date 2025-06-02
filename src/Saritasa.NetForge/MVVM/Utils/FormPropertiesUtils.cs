using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Helpers;
using Saritasa.Tools.Common.Extensions;

namespace Saritasa.NetForge.MVVM.Utils;

/// <summary>
/// Helps to work with properties on create and edit forms.
/// </summary>
public static class FormPropertiesUtils
{
    /// <summary>
    /// Removes owned property navigation and adds all its properties to given properties collection.
    /// </summary>
    /// <param name="instance">Used to create owned navigation instance when it is null.</param>
    /// <param name="properties">Owned navigations' properties will be added here.</param>
    /// <param name="navigations">
    /// Navigations. If there is no owned navigations here, then this method do nothing.
    /// </param>
    public static void HandleOwnedNavigations(
        object instance, ICollection<PropertyMetadataDto> properties, List<NavigationMetadataDto> navigations)
    {
        var ownedNavigations = navigations.Where(navigation => navigation.IsOwnership);
        foreach (var navigation in ownedNavigations)
        {
            OwnedEntityHelper.EnsureNavigationInstance(instance, navigation);

            var ownedProperties = navigation.TargetEntityProperties.Union(navigation.TargetEntityNavigations);
            properties.Add(ownedProperties);
            properties.Remove(navigation);
        }
    }

    /// <summary>
    /// Order properties using their <see cref="PropertyMetadata.FormOrder"/>.
    /// </summary>
    /// <param name="properties">Properties to order.</param>
    /// <returns>Ordered properties.</returns>
    public static List<PropertyMetadataDto> OrderProperties(ICollection<PropertyMetadataDto> properties)
    {
        return properties
            .OrderByDescending(property => property.FormOrder.HasValue)
            .ThenBy(property => property.FormOrder)
            .ToList();
    }
}

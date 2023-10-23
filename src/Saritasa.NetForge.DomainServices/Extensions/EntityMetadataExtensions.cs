using System.ComponentModel;
using System.Reflection;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;

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
    /// <param name="adminOptions">Options to apply for the entity metadata.</param>
    public static void ApplyOptions(this EntityMetadata entityMetadata, AdminOptions adminOptions)
    {
        var entityOptions =
            adminOptions.EntityOptionsList.FirstOrDefault(options => options.EntityType == entityMetadata.ClrType);

        if (entityOptions == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(entityOptions.Description))
        {
            entityMetadata.Description = entityOptions.Description;
        }

        if (!string.IsNullOrEmpty(entityOptions.Name))
        {
            entityMetadata.Name = entityOptions.Name;
        }

        if (!string.IsNullOrEmpty(entityOptions.PluralName))
        {
            entityMetadata.PluralName = entityOptions.PluralName;
        }

        if (entityOptions.IsHidden)
        {
            entityMetadata.IsHidden = entityOptions.IsHidden;
        }

        entityMetadata.ApplyPropertyOptions(entityOptions.PropertyOptions);
    }

    private static void ApplyPropertyOptions(
        this EntityMetadata entityMetadata, IEnumerable<PropertyOptions> propertyOptions)
    {
        foreach (var option in propertyOptions)
        {
            var property = entityMetadata.Properties.FirstOrDefault(property => property.Name == option.PropertyName);

            if (property is not null)
            {
                property.IsHidden = option.IsHidden;

                if (!string.IsNullOrEmpty(option.DisplayName))
                {
                    property.DisplayName = option.DisplayName;
                }

                if (!string.IsNullOrEmpty(option.Description))
                {
                    property.Description = option.Description;
                }

                if (option.Position.HasValue)
                {
                    property.Position = option.Position.Value;
                }
            }
        }
    }

    /// <summary>
    /// Applies entity-specific attributes to the given <see cref="EntityMetadata"/>.
    /// </summary>
    /// <param name="entityMetadata">The metadata of the entity to which attributes are applied.</param>
    public static void ApplyEntityAttributes(this EntityMetadata entityMetadata)
    {
        entityMetadata.Properties.ApplyPropertyAttributes();

        // Try to get the description from the System.ComponentModel.DisplayNameAttribute.
        var displayNameAttribute = entityMetadata.ClrType?.GetCustomAttribute<DisplayNameAttribute>();

        if (!string.IsNullOrEmpty(displayNameAttribute?.DisplayName))
        {
            entityMetadata.Name = displayNameAttribute.DisplayName;
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

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.Name))
        {
            entityMetadata.Name = netForgeEntityAttribute.Name;
        }

        if (!string.IsNullOrEmpty(netForgeEntityAttribute.PluralName))
        {
            entityMetadata.PluralName = netForgeEntityAttribute.PluralName;
        }

        if (netForgeEntityAttribute.IsHidden)
        {
            entityMetadata.IsHidden = netForgeEntityAttribute.IsHidden;
        }
    }

    private static void ApplyPropertyAttributes(this IEnumerable<PropertyMetadata> properties)
    {
        foreach (var property in properties)
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
                continue;
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

            if (netForgePropertyAttribute.Position >= 0)
            {
                property.Position = netForgePropertyAttribute.Position;
            }
        }
    }
}

using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's navigation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgeNavigationAttribute : Attribute
{
    /// <inheritdoc cref="NavigationMetadata.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="NavigationMetadata.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="NavigationMetadata.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="NavigationMetadata.Order"/>
    /// <remarks>
    /// We override default value with <c>-1</c>,
    /// because we need to handle situation when user chose property order is <c>0</c>.
    /// </remarks>
    public int Order { get; set; } = -1;

    /// <inheritdoc cref="NavigationMetadata.DisplayFormat"/>
    public string? DisplayFormat { get; set; }
}

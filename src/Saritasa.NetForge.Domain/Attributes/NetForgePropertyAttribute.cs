using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgePropertyAttribute : Attribute
{
    /// <inheritdoc cref="PropertyMetadata.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadata.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Order"/>
    /// <remarks>
    /// We override default value with <c>-1</c>,
    /// because we need to handle situation when user chose property order is <c>0</c>.
    /// </remarks>
    public int Order { get; set; } = -1;

    /// <inheritdoc cref="PropertyMetadata.DisplayFormat"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadata.SearchType"/>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <inheritdoc cref="PropertyMetadata.IsSortable"/>
    public bool IsSortable { get; set; }

    /// <inheritdoc cref="PropertyMetadata.EmptyValueDisplay"/>
    public string EmptyValueDisplay { get; set; } = string.Empty;
}

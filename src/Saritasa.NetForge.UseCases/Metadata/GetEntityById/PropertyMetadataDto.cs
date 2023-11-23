using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// DTO for <see cref="PropertyMetadata"/>.
/// </summary>
public record PropertyMetadataDto
{
    /// <inheritdoc cref="PropertyMetadata.Name"/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.DisplayName"/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.Description"/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.IsPrimaryKey"/>
    public bool IsPrimaryKey { get; set; }

    /// <inheritdoc cref="PropertyMetadata.SearchType"/>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <inheritdoc cref="PropertyMetadata.Order"/>
    public int? Order { get; set; }

    /// <inheritdoc cref="PropertyMetadata.DisplayFormat"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadata.FormatProvider"/>
    public IFormatProvider? FormatProvider { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsCalculatedProperty"/>
    public bool IsCalculatedProperty { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsSortable"/>
    public bool IsSortable { get; set; }

    /// <summary>
    /// Whether the property is navigation or not.
    /// </summary>
    public bool IsNavigation { get; set; }

    /// <summary>
    /// Whether the property is navigation collection or not.
    /// </summary>
    public bool IsNavigationCollection { get; set; }

    /// <inheritdoc cref="NavigationMetadata.TargetEntityProperties"/>
    public ICollection<PropertyMetadataDto> TargetEntityProperties { get; set; } = new List<PropertyMetadataDto>();

    /// <inheritdoc cref="PropertyMetadata.EmptyValueDisplay"/>
    public string EmptyValueDisplay { get; set; } = string.Empty;
}

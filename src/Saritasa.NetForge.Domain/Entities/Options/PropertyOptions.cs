using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Options for entity property.
/// </summary>
public class PropertyOptions
{
    /// <inheritdoc cref="PropertyMetadata.Name"/>
    public string PropertyName { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadata.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Order"/>
    public int? Order { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Order"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadata.FormatProvider"/>
    public IFormatProvider? FormatProvider { get; set; }

    /// <inheritdoc cref="PropertyMetadata.SearchType"/>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <inheritdoc cref="PropertyMetadata.IsSortable"/>
    public bool IsSortable { get; set; }

    /// <inheritdoc cref="PropertyMetadata.EmptyValueDisplay"/>
    public string EmptyDefaultValue { get; set; } = string.Empty;
}

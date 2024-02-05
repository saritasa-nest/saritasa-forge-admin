using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// DTO for <see cref="PropertyMetadata"/>.
/// </summary>
public record PropertyMetadataDto
{
    /// <inheritdoc cref="PropertyMetadataBase.Name"/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.DisplayName"/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.Description"/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.IsPrimaryKey"/>
    public bool IsPrimaryKey { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.ClrType"/>
    public Type? ClrType { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.SearchType"/>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <inheritdoc cref="PropertyMetadataBase.Order"/>
    public int? Order { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.DisplayFormat"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.FormatProvider"/>
    public IFormatProvider? FormatProvider { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsCalculatedProperty"/>
    public bool IsCalculatedProperty { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsSortable"/>
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

    /// <inheritdoc cref="PropertyMetadataBase.EmptyValueDisplay"/>
    public string EmptyValueDisplay { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsHiddenFromListView"/>
    public bool IsHiddenFromListView { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.IsHiddenFromDetails"/>
    public bool IsHiddenFromDetails { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.DisplayAsHtml"/>
    public bool DisplayAsHtml { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsValueGeneratedOnAdd"/>
    public bool IsValueGeneratedOnAdd { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsValueGeneratedOnUpdate"/>
    public bool IsValueGeneratedOnUpdate { get; set; }

    /// <inheritdoc cref="PropertyMetadata.IsPathToImage"/>
    public bool IsPathToImage { get; set; }

    /// <inheritdoc cref="PropertyMetadata.ImageFolder"/>
    public string ImageFolder { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadata.IsBase64Image"/>
    public bool IsBase64Image { get; set; }
}

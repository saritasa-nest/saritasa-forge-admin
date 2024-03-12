using System.Reflection;
using Saritasa.NetForge.Domain.Enums;

namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Base class for property metadata.
/// </summary>
public abstract class PropertyMetadataBase
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The type of the property.
    /// </summary>
    public Type? ClrType { get; set; }

    /// <summary>
    /// The display name of the property. If not empty this name will be displayed instead of <see cref="Name"/>.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The description of the property.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The <see cref="PropertyInfo"/> representing the property in .NET reflection.
    /// </summary>
    public PropertyInfo? PropertyInformation { get; set; }

    /// <summary>
    /// The order of the property.
    /// </summary>
    public int? Order { get; set; }

    /// <summary>
    /// Whether the property is hidden from the view.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Whether the property is hidden from the list view.
    /// </summary>
    public bool IsHiddenFromListView { get; set; }

    /// <summary>
    /// Whether the property is hidden from the details.
    /// </summary>
    public bool IsHiddenFromDetails { get; set; }

    /// <summary>
    /// Whether the property is excluded from the query.
    /// </summary>
    public bool IsExcludedFromQuery { get; set; }

    /// <summary>
    /// Display format of the property value.
    /// </summary>
    public string? DisplayFormat { get; set; }

    /// <summary>
    /// Format provider for the property value.
    /// </summary>
    public IFormatProvider? FormatProvider { get; set; }

    /// <summary>
    /// Search type.
    /// </summary>
    public SearchType SearchType { get; set; } = SearchType.None;

    /// <summary>
    /// Whether the property is sortable.
    /// </summary>
    public bool IsSortable { get; set; }

    /// <summary>
    /// Display this value when the value of property is empty.
    /// </summary>
    public string EmptyValueDisplay { get; set; } = string.Empty;

    /// <summary>
    /// Whether the property is rendered as HTML.
    /// </summary>
    public bool DisplayAsHtml { get; set; }

    /// <summary>
    /// Whether the property is readonly.
    /// </summary>
    public bool IsReadOnly { get; set; }
}

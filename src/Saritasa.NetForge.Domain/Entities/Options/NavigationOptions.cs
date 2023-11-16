using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Options for entity navigation.
/// </summary>
public class NavigationOptions
{
    /// <inheritdoc cref="PropertyMetadataBase.Name"/>
    public string NavigationName { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.Order"/>
    public int? Order { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.DisplayFormat"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="PropertyMetadataBase.FormatProvider"/>
    public IFormatProvider? FormatProvider { get; set; }
}

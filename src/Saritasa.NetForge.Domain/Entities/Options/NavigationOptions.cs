using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Options for entity navigation.
/// </summary>
public class NavigationOptions
{
    /// <inheritdoc cref="NavigationMetadata.Name"/>
    public string NavigationName { get; set; } = string.Empty;

    /// <inheritdoc cref="NavigationMetadata.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="NavigationMetadata.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="NavigationMetadata.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="NavigationMetadata.Order"/>
    public int? Order { get; set; }

    /// <inheritdoc cref="NavigationMetadata.Order"/>
    public string? DisplayFormat { get; set; }

    /// <inheritdoc cref="NavigationMetadata.FormatProvider"/>
    public IFormatProvider? FormatProvider { get; set; }
}

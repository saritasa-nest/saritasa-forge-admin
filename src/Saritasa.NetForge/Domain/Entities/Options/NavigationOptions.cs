using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Navigation options.
/// </summary>
public class NavigationOptions
{
    /// <inheritdoc cref="PropertyMetadataBase.Name"/>
    public string PropertyName { get; set; } = string.Empty;

    /// <inheritdoc cref="PropertyMetadataBase.FormOrder"/>
    public int? FormOrder { get; set; }

    /// <summary>
    /// Property options for the navigation properties.
    /// </summary>
    public ICollection<PropertyOptions> PropertyOptions { get; set; } = [];

    /// <summary>
    /// Property options for the navigation calculated properties.
    /// </summary>
    public ICollection<CalculatedPropertyOptions> CalculatedPropertyOptions { get; set; } = [];

    /// <summary>
    /// Navigation options for inner navigations.
    /// </summary>
    public ICollection<NavigationOptions> NavigationsOptions { get; set; } = [];
}

namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Navigation options.
/// </summary>
public class NavigationOptions : PropertyOptions
{
    /// <summary>
    /// Property options for the navigation properties.
    /// </summary>
    public ICollection<PropertyOptions> PropertyOptions { get; set; } = new List<PropertyOptions>();
}

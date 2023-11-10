using System.Reflection;

namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Represents metadata about a navigation of an entity model.
/// </summary>
public class NavigationMetadata
{
    /// <summary>
    /// The name of the navigation.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the navigation. If not empty this name will be displayed instead of <see cref="Name"/>.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The description of the navigation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Whether the navigation is hidden from the view.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Whether this navigation is collection.
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// Target navigation entity's properties.
    /// </summary>
    public List<PropertyMetadata> TargetEntityProperties { get; set; } = new();

    /// <summary>
    /// The <see cref="PropertyInfo"/> representing the property in .NET reflection.
    /// </summary>
    public PropertyInfo? PropertyInformation { get; set; }
}

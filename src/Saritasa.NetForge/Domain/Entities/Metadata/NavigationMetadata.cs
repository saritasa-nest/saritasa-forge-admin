namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Represents metadata about a navigation of an entity model.
/// </summary>
public class NavigationMetadata : PropertyMetadataBase
{
    /// <summary>
    /// Whether this navigation is collection.
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// If true, the navigation cannot exist without its parent entity.
    /// </summary>
    /// <remarks>
    /// See documentation <see href="https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities"/>.
    /// </remarks>
    public bool IsOwnership { get; set; }

    /// <summary>
    /// Target navigation entity's properties.
    /// </summary>
    public List<PropertyMetadata> TargetEntityProperties { get; set; } = [];

    /// <summary>
    /// Target navigation entity's navigations.
    /// </summary>
    public List<NavigationMetadata> TargetEntityNavigations { get; set; } = [];
}

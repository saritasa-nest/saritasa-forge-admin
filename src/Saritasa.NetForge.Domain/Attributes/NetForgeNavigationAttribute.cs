namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's navigation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgeNavigationAttribute : NetForgePropertyAttributeBase
{
    /// <summary>
    /// Whether include the navigation to an entity.
    /// </summary>
    public bool IsIncluded { get; set; }
}

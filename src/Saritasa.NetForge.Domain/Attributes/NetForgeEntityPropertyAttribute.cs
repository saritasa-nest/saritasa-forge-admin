namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgeEntityPropertyAttribute : Attribute
{
    /// <summary>
    /// Whether the entity is hidden from the view.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Name that will be displayed instead of default property name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }
}

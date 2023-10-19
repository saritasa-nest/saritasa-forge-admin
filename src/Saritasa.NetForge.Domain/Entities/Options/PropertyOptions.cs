namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Options for entity property.
/// </summary>
public class PropertyOptions
{
    /// <summary>
    /// Property name.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Is property must be hidden.
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

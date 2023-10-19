using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's property.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgePropertyAttribute : Attribute
{
    /// <inheritdoc cref="PropertyMetadata.IsHidden"/>
    public bool IsHidden { get; set; }

    /// <inheritdoc cref="PropertyMetadata.DisplayName"/>
    public string? DisplayName { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="PropertyMetadata.Position"/>
    public short Position { get; set; }
}

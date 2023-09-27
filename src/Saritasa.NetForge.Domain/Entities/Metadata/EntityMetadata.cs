namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Metadata of the Database Entity.
/// </summary>
public class EntityMetadata
{
    /// <summary>
    /// Name of the entity to display.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Plural name of the entity.
    /// </summary>
    public string PluralName { get; set; } = string.Empty;

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Entity CLR type.
    /// </summary>
    public Type? ClrType { get; set; }

    /// <summary>
    /// Whether the entity can be edited.
    /// </summary>
    public bool IsEditable { get; set; } = true;

    /// <summary>
    /// A collection of properties metadata associated with this entity.
    /// </summary>
    public IEnumerable<PropertyMetadata> Properties = new List<PropertyMetadata>();
}

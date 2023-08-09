namespace Saritasa.NetForge.Domain.Entities;

/// <summary>
/// Metadata of the Database Entity.
/// </summary>
public class EntityMetadata
{
    /// <summary>
    /// Name of the entity to display.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Plural name of the entity.
    /// </summary>
    public string? PluralName { get; set; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Entity CLR type.
    /// </summary>
    public Type? ClrType { get; set; }

    /// <summary>
    /// Whether the entity can be edited.
    /// </summary>
    public bool IsEditable { get; set; } = true;
}

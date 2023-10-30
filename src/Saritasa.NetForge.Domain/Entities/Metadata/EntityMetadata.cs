namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Metadata of the Database Entity.
/// </summary>
public class EntityMetadata
{
    /// <summary>
    /// The id of the entity.
    /// </summary>
    public Guid Id { get; set; }

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
    /// Whether the entity is hidden.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// A collection of properties metadata associated with this entity.
    /// </summary>
    public ICollection<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();

    public Func<object?, IQueryable<object>, string, IQueryable<object>> CustomSearch { get; set; }
}

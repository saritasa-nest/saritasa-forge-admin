namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Configure entity in the admin panel.
/// </summary>
public class EntityOptions
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityOptions(Type entityType)
    {
        EntityType = entityType;
    }

    /// <summary>
    /// Type of the Entity to configure.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

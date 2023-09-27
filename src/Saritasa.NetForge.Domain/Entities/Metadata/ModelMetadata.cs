namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Represents the metadata of the DB model.
/// </summary>
public class ModelMetadata
{
    /// <summary>
    /// Name of the model.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Type of the model.
    /// </summary>
    public Type? ClrType { get; set; }

    /// <summary>
    /// List of related entities metadata.
    /// </summary>
    public IList<EntityMetadata> Entities { get; set; } = new List<EntityMetadata>();
}

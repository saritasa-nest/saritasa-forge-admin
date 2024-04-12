using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Domain.Entities.Metadata;

/// <summary>
/// Represents metadata about a property of an entity model.
/// </summary>
public class PropertyMetadata : PropertyMetadataBase
{
    /// <summary>
    /// Whether the property is a primary key.
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// Whether the property is a foreign key.
    /// </summary>
    public bool IsForeignKey { get; set; }

    /// <summary>
    /// Whether the property is editable.
    /// </summary>
    public bool IsEditable { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether this is a shadow property (does not have a
    /// corresponding property in the entity class).
    /// </summary>
    public bool IsShadow { get; set; }

    /// <summary>
    /// Whether the property value is generated on entity database create.
    /// </summary>
    public bool IsValueGeneratedOnAdd { get; set; }

    /// <summary>
    /// Whether the property value is generated on entity database update.
    /// </summary>
    public bool IsValueGeneratedOnUpdate { get; set; }

    /// <summary>
    /// Whether this property is calculated.
    /// </summary>
    public bool IsCalculatedProperty { get; set; }

    /// <summary>
    /// Whether this property is image.
    /// </summary>
    public bool IsImage { get; set; }

    /// <summary>
    /// Upload file strategy.
    /// </summary>
    public IUploadFileStrategy? UploadFileStrategy { get; set; }
}

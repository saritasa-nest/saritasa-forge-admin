using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Mvvm.ViewModels.Details;

/// <summary>
/// Details model.
/// </summary>
public class DetailsModel
{
    /// <inheritdoc cref="EntityMetadata.Id"/>
    public Guid Id { get; set; }

    /// <inheritdoc cref="EntityMetadata.Name"/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.PluralName"/>
    public string PluralName { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.Description"/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.Properties"/>
    public ICollection<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();
}

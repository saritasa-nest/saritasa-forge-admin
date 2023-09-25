using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.Mvvm.ViewModels.Metadata;

/// <summary>
/// Model for the entities.
/// </summary>
public class EntitiesModel
{
    /// <summary>
    /// Collection of entities metadata.
    /// </summary>
    public IEnumerable<EntityMetadataDto> EntitiesMetadata { get; set; } = new List<EntityMetadataDto>();
}

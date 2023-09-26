using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Provides contract for getting ORM-specific entities metadata.
/// </summary>
public interface IOrmMetadataService
{
    /// <summary>
    /// Gets metadata of all entities.
    /// </summary>
    IEnumerable<EntityMetadata> GetEntities();
}

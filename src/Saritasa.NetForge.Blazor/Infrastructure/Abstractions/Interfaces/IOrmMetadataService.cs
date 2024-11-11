using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Provides a contract for retrieving ORM-specific metadata for entities.
/// </summary>
public interface IOrmMetadataService
{
    /// <summary>
    /// Get metadata for entities.
    /// </summary>
    IEnumerable<EntityMetadata> GetMetadata();
}

using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Provides a contract for retrieving ORM-specific metadata for models.
/// </summary>
public interface IOrmMetadataService
{
    /// <summary>
    /// Gets metadata for models.
    /// </summary>
    IEnumerable<EntityMetadata> GetMetadata();
}

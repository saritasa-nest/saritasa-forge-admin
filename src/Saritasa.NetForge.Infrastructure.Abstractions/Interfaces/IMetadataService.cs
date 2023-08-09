using Saritasa.NetForge.Domain.Entities;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Provides contract for getting entities metadata.
/// </summary>
public interface IMetadataService
{
    /// <summary>
    /// Gets metadata of all entities.
    /// </summary>
    /// <returns></returns>
    IEnumerable<EntityMetadata> GetEntities();
}

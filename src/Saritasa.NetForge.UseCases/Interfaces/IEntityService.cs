using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.Tools.Common.Pagination;

namespace Saritasa.NetForge.UseCases.Interfaces;

/// <summary>
/// Handle operations related to entities.
/// </summary>
public interface IEntityService
{
    /// <summary>
    /// Search for entities metadata.
    /// </summary>
    /// <param name="cancellationToken">Token for cancelling async operation.</param>
    Task<IEnumerable<EntityMetadataDto>> SearchEntitiesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.Id"/>.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="cancellationToken">Token for cancelling async operation.</param>
    Task<GetEntityByIdDto> GetEntityByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get the data for the specific entity type.
    /// </summary>
    /// <param name="entityType">Entity type to search data. For example, search all data for entity with type <c>Address</c>.</param>
    /// <param name="properties">Entity properties metadata to be included in returned data.</param>
    /// <param name="searchOptions">Search options.</param>
    Task<PagedListMetadataDto<object>> SearchDataForEntityAsync(Type? entityType,
        ICollection<PropertyMetadata> properties, SearchOptions searchOptions);
}

using MediatR;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.Tools.Common.Pagination;

namespace Saritasa.NetForge.UseCases.Metadata.SearchDataForEntity;

/// <summary>
/// Query to search data related to some entity.
/// </summary>
/// <param name="EntityType">
/// Entity type to search data. For example, search all data for entity with type <c>Address</c>.
/// </param>
/// <param name="SearchOptions">Search options.</param>
public record SearchDataForEntityQuery(
        Type? EntityType, SearchOptions SearchOptions, ICollection<PropertyMetadata> Properties)
    : IRequest<PagedListMetadataDto<object>>;

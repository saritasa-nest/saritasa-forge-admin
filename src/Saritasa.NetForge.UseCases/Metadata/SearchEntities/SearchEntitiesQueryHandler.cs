using AutoMapper;
using MediatR;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.Services;

namespace Saritasa.NetForge.UseCases.Metadata.SearchEntities;

/// <summary>
/// Handler for <see cref="SearchEntitiesQuery"/>.
/// </summary>
internal class SearchEntitiesQueryHandler : IRequestHandler<SearchEntitiesQuery, IEnumerable<EntityMetadataDto>>
{
    private readonly IMapper mapper;
    private readonly AdminMetadataService adminMetadataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchEntitiesQueryHandler(AdminMetadataService adminMetadataService, IMapper mapper)
    {
        this.mapper = mapper;
        this.adminMetadataService = adminMetadataService;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<EntityMetadataDto>> Handle(SearchEntitiesQuery request,
        CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService.GetMetadata()
            .Where(entityMetadata => !entityMetadata.IsHidden);
        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(metadata));
    }
}

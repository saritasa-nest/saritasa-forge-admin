using AutoMapper;
using MediatR;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.UseCases.Metadata.SearchEntities;

/// <summary>
/// Handler for <see cref="SearchEntitiesQuery"/>.
/// </summary>
internal class SearchEntitiesQueryHandler : IRequestHandler<SearchEntitiesQuery, IEnumerable<EntityMetadataDto>>
{
    private readonly IMetadataService metadataService;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchEntitiesQueryHandler(IMetadataService metadataService, IMapper mapper)
    {
        this.metadataService = metadataService;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<EntityMetadataDto>> Handle(SearchEntitiesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = metadataService.GetEntities();
        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(entities));
    }
}

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
    private readonly AdminService adminService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchEntitiesQueryHandler(AdminService adminService, IMapper mapper)
    {
        this.mapper = mapper;
        this.adminService = adminService;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<EntityMetadataDto>> Handle(SearchEntitiesQuery request,
        CancellationToken cancellationToken)
    {
        var metadata = adminService.GetMetadata()
            .Where(entityMetadata => !entityMetadata.IsHidden);
        return Task.FromResult(mapper.Map<IEnumerable<EntityMetadataDto>>(metadata));
    }
}

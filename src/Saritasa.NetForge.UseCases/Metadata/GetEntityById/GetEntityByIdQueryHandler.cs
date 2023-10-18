using AutoMapper;
using MediatR;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.Services;
using Saritasa.Tools.Common.Pagination;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// Handler for <see cref="GetEntityByIdQuery"/>.
/// </summary>
internal class GetEntityByIdQueryHandler : IRequestHandler<GetEntityByIdQuery, GetEntityByIdDto>
{
    private readonly IMapper mapper;
    private readonly AdminMetadataService adminMetadataService;
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdQueryHandler(AdminMetadataService adminMetadataService, IMapper mapper, IOrmDataService dataService)
    {
        this.mapper = mapper;
        this.dataService = dataService;
        this.adminMetadataService = adminMetadataService;
    }

    /// <inheritdoc/>
    public Task<GetEntityByIdDto> Handle(GetEntityByIdQuery request, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .First(entityMetadata => entityMetadata.Id == request.Id);

        var data = dataService.GetData(metadata.ClrType).OfType<object>().ToList();

        var searchOptions = request.PageQueryFilter;

        var pagedList = PagedListFactory.FromSource(data, searchOptions.Page, searchOptions.PageSize);
        var pagedListMetadata = pagedList.ToMetadataObject();

        var metadataDto = mapper.Map<GetEntityByIdDto>(metadata);

        metadataDto = metadataDto with { Data = data };
        return Task.FromResult(metadataDto);
    }
}

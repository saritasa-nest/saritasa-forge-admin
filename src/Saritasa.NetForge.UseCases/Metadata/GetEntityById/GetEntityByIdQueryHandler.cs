using AutoMapper;
using MediatR;
using Saritasa.NetForge.UseCases.Metadata.Services;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// Handler for <see cref="GetEntityByIdQuery"/>.
/// </summary>
internal class GetEntityByIdQueryHandler : IRequestHandler<GetEntityByIdQuery, GetEntityByIdDto>
{
    private readonly IMapper mapper;
    private readonly AdminMetadataService adminMetadataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GetEntityByIdQueryHandler(AdminMetadataService adminMetadataService, IMapper mapper)
    {
        this.mapper = mapper;
        this.adminMetadataService = adminMetadataService;
    }

    /// <inheritdoc/>
    public async Task<GetEntityByIdDto> Handle(GetEntityByIdQuery request, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService.GetMetadata()
            .FirstOrDefault(entityMetadata => entityMetadata.Id == request.Id);

        return mapper.Map<GetEntityByIdDto>(metadata);
    }
}

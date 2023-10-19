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
    public GetEntityByIdQueryHandler(IMapper mapper, AdminMetadataService adminMetadataService)
    {
        this.mapper = mapper;
        this.adminMetadataService = adminMetadataService;
    }

    /// <inheritdoc/>
    public Task<GetEntityByIdDto> Handle(GetEntityByIdQuery request, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .First(entityMetadata => entityMetadata.Id == request.Id);

        var metadataDto = mapper.Map<GetEntityByIdDto>(metadata);

        return Task.FromResult(metadataDto);
    }
}

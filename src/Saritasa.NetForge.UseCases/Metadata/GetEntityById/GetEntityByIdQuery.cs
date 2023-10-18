using MediatR;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Common;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// Query to get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.Id"/>.
/// </summary>
public record GetEntityByIdQuery(Guid Id, PageQueryFilter PageQueryFilter) : IRequest<GetEntityByIdDto>;


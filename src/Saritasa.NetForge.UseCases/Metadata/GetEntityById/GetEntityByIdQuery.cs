using MediatR;
using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// Query to get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.Id"/>.
/// </summary>
/// <param name="Id">Entity identifier.</param>
public record GetEntityByIdQuery(Guid Id) : IRequest<GetEntityByIdDto>;

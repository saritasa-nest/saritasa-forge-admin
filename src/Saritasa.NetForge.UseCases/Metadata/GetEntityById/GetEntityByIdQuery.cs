using MediatR;
using Saritasa.NetForge.Domain.Entities.Metadata;

namespace Saritasa.NetForge.UseCases.Metadata.GetEntityById;

/// <summary>
/// Query to get <see cref="EntityMetadata"/> by <see cref="EntityMetadata.Id"/>.
/// </summary>
public record GetEntityByIdQuery : IRequest<GetEntityByIdDto>
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public Guid Id { get; set; }
}

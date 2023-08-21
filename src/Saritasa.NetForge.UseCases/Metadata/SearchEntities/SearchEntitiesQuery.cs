using MediatR;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.UseCases.Metadata.SearchEntities;

/// <summary>
/// Search for entities metadata.
/// </summary>
public class SearchEntitiesQuery : IRequest<IEnumerable<EntityMetadataDto>>
{
}

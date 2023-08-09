using MediatR;
using Saritasa.NetForge.DomainServices.DTOs;

namespace Saritasa.NetForge.UseCases.Metadata.SearchEntities;

/// <summary>
/// Search for entities metadata.
/// </summary>
public class SearchEntitiesQuery : IRequest<IEnumerable<EntityMetadataDto>>
{
}

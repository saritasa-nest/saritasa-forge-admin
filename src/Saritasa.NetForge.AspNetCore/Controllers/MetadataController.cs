using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saritasa.NetForge.DomainServices.DTOs;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.AspNetCore.Controllers;

/// <summary>
/// Gets database metadata.
/// </summary>
public class MetadataController : Controller
{
    private readonly IMediator mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    public MetadataController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Gets the list of entities.
    /// </summary>
    public async Task<IEnumerable<EntityMetadataDto>> GetEntities(CancellationToken cancellationToken)
    {
        return await mediator.Send(new SearchEntitiesQuery(), cancellationToken);
    }
}

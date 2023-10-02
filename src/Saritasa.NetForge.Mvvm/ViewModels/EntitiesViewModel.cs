using MediatR;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// ViewModel representing entities metadata in the admin panel.
/// </summary>
public class EntitiesViewModel : BaseViewModel
{
    private readonly IMediator mediator;

    /// <summary>
    /// Collection of entities metadata.
    /// </summary>
    public IEnumerable<EntityMetadataDto> EntitiesMetadata { get; set; } = new List<EntityMetadataDto>();

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntitiesViewModel(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Load entities metadata.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        await GetEntitiesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieve entities metadata from the backend.
    /// </summary>
    private async Task GetEntitiesAsync(CancellationToken cancellationToken)
    {
        var entitiesMetadataDto = await mediator.Send(new SearchEntitiesQuery(), cancellationToken);
        EntitiesMetadata = entitiesMetadataDto;
    }
}

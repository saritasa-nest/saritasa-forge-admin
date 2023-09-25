using MediatR;
using Saritasa.NetForge.UseCases.Metadata.SearchEntities;

namespace Saritasa.NetForge.Mvvm.ViewModels.Metadata;

/// <summary>
/// Index view model.
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly IMediator mediator;

    /// <summary>
    /// Model instance.
    /// </summary>
    public MainModel Model { get; } = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    public MainViewModel(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        await GetEntitiesAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities metadata.
    /// </summary>
    private async Task GetEntitiesAsync(CancellationToken cancellationToken)
    {
        var entitiesMetadataDto = await mediator.Send(new SearchEntitiesQuery(), cancellationToken);
        Model.EntitiesMetadata = entitiesMetadataDto;
    }
}

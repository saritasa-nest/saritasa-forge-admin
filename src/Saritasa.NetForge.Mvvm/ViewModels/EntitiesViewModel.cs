using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// ViewModel representing entities metadata in the admin panel.
/// </summary>
public class EntitiesViewModel : BaseViewModel
{
    private readonly IEntityService entityService;

    /// <summary>
    /// Collection of entities metadata.
    /// </summary>
    public IEnumerable<EntityMetadataDto> EntitiesMetadata { get; set; } = new List<EntityMetadataDto>();

    /// <summary>
    /// Constructor.
    /// </summary>
    public EntitiesViewModel(IEntityService entityService)
    {
        this.entityService = entityService;
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
        var entitiesMetadataDto = await entityService.SearchEntitiesAsync(cancellationToken);
        EntitiesMetadata = entitiesMetadataDto;
    }
}

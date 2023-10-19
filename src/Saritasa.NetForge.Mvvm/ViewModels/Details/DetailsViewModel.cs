using AutoMapper;
using MediatR;
using MudBlazor;
using Saritasa.NetForge.UseCases.Common;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.UseCases.Metadata.SearchDataForEntity;

namespace Saritasa.NetForge.Mvvm.ViewModels.Details;

/// <summary>
/// ViewModel representing details of an entity.
/// </summary>
public class DetailsViewModel : BaseViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DetailsViewModel(Guid id, IMediator mediator, IMapper mapper)
    {
        Model = new DetailsModel { Id = id };

        this.mediator = mediator;
        this.mapper = mapper;
    }

    /// <summary>
    /// Details model.
    /// </summary>
    public DetailsModel Model { get; private set; }

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        var entity = await mediator.Send(new GetEntityByIdQuery(Model.Id), cancellationToken);

        Model = mapper.Map<DetailsModel>(entity);
    }

    /// <summary>
    /// Loads entity's data to data grid.
    /// </summary>
    /// <param name="gridState">Information about grid state. For example, current page, page size.</param>
    /// <returns>Grid data collection populated with entity's data.</returns>
    public async Task<GridData<object>> LoadEntityGridDataAsync(GridState<object> gridState)
    {
        var searchOptions = new SearchOptions
        {
            Page = gridState.Page + 1,
            PageSize = gridState.PageSize
        };

        var entityData = await mediator.Send(new SearchDataForEntityQuery(Model.ClrType, searchOptions));

        var data = new GridData<object>
        {
            Items = entityData.Items,
            TotalItems = entityData.Metadata.TotalCount
        };

        return data;
    }
}

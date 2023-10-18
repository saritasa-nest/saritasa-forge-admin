using AutoMapper;
using MediatR;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

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
        // TODO
        var pageQueryFilter = new PageQueryFilter { Page = 1, PageSize = 15 };
        var entity = await mediator.Send(new GetEntityByIdQuery(Model.Id, pageQueryFilter), cancellationToken);

        Model = mapper.Map<DetailsModel>(entity);
    }
}

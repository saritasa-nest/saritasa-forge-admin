using AutoMapper;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;

/// <summary>
/// View model for create entity page.
/// </summary>
public class CreateEntityViewModel : BaseViewModel
{
    /// <summary>
    /// Entity details model.
    /// </summary>
    public CreateEntityModel Model { get; private set; }

    private readonly IEntityService entityService;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityViewModel(string stringId, IEntityService entityService, IMapper mapper)
    {
        Model = new CreateEntityModel { StringId = stringId };

        this.entityService = entityService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Whether the entity exists.
    /// </summary>
    public bool IsEntityExists { get; private set; } = true;

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);
            Model = mapper.Map<CreateEntityModel>(entity);
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
    }
}

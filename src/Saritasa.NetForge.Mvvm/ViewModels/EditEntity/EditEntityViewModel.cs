using AutoMapper;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.Mvvm.ViewModels.EditEntity;

/// <summary>
/// View model for edit entity page.
/// </summary>
public class EditEntityViewModel : BaseViewModel
{
    /// <summary>
    /// Entity details model.
    /// </summary>
    public EditEntityModel Model { get; private set; }

    private readonly IEntityService entityService;
    private readonly IMapper mapper;
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EditEntityViewModel(
        string stringId, IEntityService entityService, IMapper mapper, IOrmDataService dataService)
    {
        Model = new EditEntityModel { StringId = stringId };

        this.entityService = entityService;
        this.mapper = mapper;
        this.dataService = dataService;
    }

    /// <summary>
    /// Whether the entity exists.
    /// </summary>
    public bool IsEntityExists { get; private set; } = true;

    /// <summary>
    /// Entity model.
    /// </summary>
    public object EntityModel { get; set; } = null!;

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);
            Model = mapper.Map<EditEntityModel>(entity);
            //EntityModel = Activator.CreateInstance(Model.ClrType!)!;
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
    }

    /// <summary>
    /// Updates entity.
    /// </summary>
    public async Task UpdateEntityAsync()
    {
       await dataService.UpdateAsync(EntityModel, Model.ClrType!, CancellationToken);
    }
}

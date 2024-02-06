using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
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
    public EditEntityModel Model { get; set; }

    /// <summary>
    /// Instance primary key.
    /// </summary>
    public string InstancePrimaryKey { get; set; }

    private readonly IEntityService entityService;
    private readonly IMapper mapper;
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EditEntityViewModel(
        string stringId,
        string instancePrimaryKey,
        IEntityService entityService,
        IMapper mapper,
        IOrmDataService dataService)
    {
        Model = new EditEntityModel { StringId = stringId };
        InstancePrimaryKey = instancePrimaryKey;

        this.entityService = entityService;
        this.mapper = mapper;
        this.dataService = dataService;
    }

    /// <summary>
    /// Whether the entity exists.
    /// </summary>
    public bool IsEntityExists { get; private set; } = true;

    /// <summary>
    /// Is entity was updated.
    /// </summary>
    public bool IsUpdated { get; set; }

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);
            Model = mapper.Map<EditEntityModel>(entity);
            Model.EntityInstance = await dataService
                .GetInstanceAsync(InstancePrimaryKey, Model.ClrType!, CancellationToken);
            Model = Model with
            {
                Properties = Model.Properties
                    .Where(property => property is
                    {
                        IsCalculatedProperty: false,
                        IsValueGeneratedOnAdd: false,
                        IsValueGeneratedOnUpdate: false,
                        IsHiddenFromListView: false
                    })
                    .ToList()
            };
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
        WeakReferenceMessenger.Default.Send(new EntitySubmittedMessage());
        await dataService.UpdateAsync(Model.EntityInstance!, CancellationToken);
        IsUpdated = true;
    }
}

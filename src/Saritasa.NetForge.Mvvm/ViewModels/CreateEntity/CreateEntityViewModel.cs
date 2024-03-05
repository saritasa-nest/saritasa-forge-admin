using AutoMapper;
using Saritasa.NetForge.Domain.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;
using Saritasa.NetForge.UseCases.Interfaces;

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
    private readonly INavigationService navigationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityViewModel(
        string stringId, IEntityService entityService, IMapper mapper, INavigationService navigationService)
    {
        Model = new CreateEntityModel { StringId = stringId };

        this.entityService = entityService;
        this.mapper = mapper;
        this.navigationService = navigationService;
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
            Model.EntityInstance = Activator.CreateInstance(Model.ClrType!)!;
            Model = Model with
            {
                Properties = Model.Properties
                    .Where(property => property is
                    {
                        IsCalculatedProperty: false,
                        IsValueGeneratedOnAdd: false,
                        IsValueGeneratedOnUpdate: false
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
    /// Creates entity.
    /// </summary>
    public async Task CreateEntityAsync()
    {
        var message = WeakReferenceMessenger.Default.Send(new UploadImageMessage());

        await entityService.CreateEntityAsync(Model.EntityInstance, Model.ClrType!, CancellationToken);
        navigationService.NavigateTo<EntityDetailsViewModel>(parameters: Model.StringId);
    }
}

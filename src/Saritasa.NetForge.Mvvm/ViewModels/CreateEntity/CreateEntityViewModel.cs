using Saritasa.NetForge.Domain.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using System.ComponentModel.DataAnnotations;

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
    private readonly INavigationService navigationService;
    private readonly IFileService fileService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityViewModel(
        string stringId,
        IEntityService entityService,
        INavigationService navigationService,
        IFileService fileService)
    {
        Model = new CreateEntityModel { StringId = stringId };

        this.entityService = entityService;
        this.navigationService = navigationService;
        this.fileService = fileService;
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
            Model = MapModel(entity);
            Model.EntityInstance = Activator.CreateInstance(Model.ClrType!)!;
            Model = Model with
            {
                Properties = Model.Properties
                    .Where(property => property is
                    {
                        IsCalculatedProperty: false,
                        IsValueGeneratedOnAdd: false,
                        IsValueGeneratedOnUpdate: false,
                        IsReadOnly: false
                    })
                    .ToList()
            };
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
    }

    private CreateEntityModel MapModel(GetEntityDto entity)
    {
        return Model with
        {
            DisplayName = entity.DisplayName,
            PluralName = entity.PluralName,
            ClrType = entity.ClrType,
            Properties = entity.Properties,
        };
    }

    /// <summary>
    /// List of <see cref="FieldErrorModel"/> instances in the view model.
    /// </summary>
    public List<FieldErrorModel> FieldErrorModels { get; } = [];

    /// <summary>
    /// Creates entity.
    /// </summary>
    public async Task CreateEntityAsync()
    {
        var message = WeakReferenceMessenger.Default.Send(new UploadImageMessage());

        foreach (var image in message.ChangedFiles)
        {
            await fileService.CreateFileAsync(image.PathToFile!, image.FileContent!, CancellationToken);
        }

        var errors = new List<ValidationResult>();

        if (entityService.ValidateEntity(Model.EntityInstance, ref errors))
        {
            await entityService.CreateEntityAsync(Model.EntityInstance, Model.ClrType!, CancellationToken);
            navigationService.NavigateTo<EntityDetailsViewModel>(parameters: Model.StringId);
        }
        else
        {
            // Clear the error on the previous validation.
            FieldErrorModels.ForEach(e => e.ErrorMessage = string.Empty);

            foreach (var result in errors)
            {
                foreach (var member in result.MemberNames)
                {
                    var errorViewModel = FieldErrorModels.FirstOrDefault(e => e.Property.Name == member);
                    if (errorViewModel is null)
                    {
                        continue;
                    }

                    errorViewModel.ErrorMessage = result.ErrorMessage!;
                }
            }
        }
    }
}

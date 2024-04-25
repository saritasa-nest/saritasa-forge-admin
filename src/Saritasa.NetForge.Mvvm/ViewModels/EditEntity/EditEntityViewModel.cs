using Saritasa.NetForge.Domain.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;
using System.ComponentModel.DataAnnotations;

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
    private readonly IOrmDataService dataService;
    private readonly IFileService fileService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EditEntityViewModel(
        string stringId,
        string instancePrimaryKey,
        IEntityService entityService,
        IOrmDataService dataService,
        IFileService fileService)
    {
        Model = new EditEntityModel { StringId = stringId };
        InstancePrimaryKey = instancePrimaryKey;

        this.entityService = entityService;
        this.dataService = dataService;
        this.fileService = fileService;
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
            Model = MapModel(entity);
            Model = Model with
            {
                Properties = Model.Properties
                    .Where(property => property is
                    {
                        IsCalculatedProperty: false,
                        IsValueGeneratedOnAdd: false,
                        IsValueGeneratedOnUpdate: false,
                        IsHiddenFromListView: false,
                    })
                    .ToList()
            };

            var includedNavigationNames = Model.Properties
                .Where(property => property is NavigationMetadataDto)
                .Select(property => property.Name);

            Model.EntityInstance = await dataService
                .GetInstanceAsync(InstancePrimaryKey, Model.ClrType!, includedNavigationNames, CancellationToken);

            Model.OriginalEntityInstance = Model.EntityInstance.CloneJson();
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
    }

    private EditEntityModel MapModel(GetEntityDto entity)
    {
        return Model with
        {
            DisplayName = entity.DisplayName,
            PluralName = entity.PluralName,
            ClrType = entity.ClrType,
            Properties = entity.Properties
        };
    }

    /// <summary>
    /// List of <see cref="ComponentErrorModel"/> instances in the view model.
    /// </summary>
    public List<ComponentErrorModel> ErrorViewModels { get; } = [];

    /// <summary>
    /// Updates entity.
    /// </summary>
    public async Task UpdateEntityAsync()
    {
        var message = WeakReferenceMessenger.Default.Send(new UploadImageMessage());

        foreach (var image in message.ChangedFiles)
        {
            await fileService.CreateFileAsync(image.PathToFile!, image.FileContent!, CancellationToken);
        }

        var error = new List<ValidationResult>();

        if (entityService.ValidateEntity(Model.EntityInstance!, ref error))
        {
            await dataService.UpdateAsync(Model.EntityInstance!, Model.OriginalEntityInstance!, CancellationToken);
            IsUpdated = true;
        }
        else
        {
            // Clear the error on the previous validation.
            ErrorViewModels.ForEach(e => e.ErrorMessage = string.Empty);

            foreach (var result in error)
            {
                foreach (var member in result.MemberNames)
                {
                    var errorViewModel = ErrorViewModels.FirstOrDefault(e => e.Property.Name == member);
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

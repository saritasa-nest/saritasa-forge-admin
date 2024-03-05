using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Saritasa.NetForge.Domain.Exceptions;
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
    /// 
    /// </summary>
    public List<ComponentErrorModel> ErrorViewModels { get; } = [];

    /// <summary>
    /// Creates entity.
    /// </summary>
    public async Task CreateEntityAsync()
    {
        var results = new List<ValidationResult>();
        ValidationContext context = new ValidationContext(Model.EntityInstance, null, null);
        if (Validator.TryValidateObject(Model.EntityInstance, context, results, true))
        {
            await entityService.CreateEntityAsync(Model.EntityInstance, Model.ClrType!, CancellationToken);
            navigationService.NavigateTo<EntityDetailsViewModel>(parameters: Model.StringId);
        }
        else
        {
            ErrorViewModels.ForEach(e => e.ErrorMessage = string.Empty);

            foreach (var result in results)
            {
                foreach (var member in result.MemberNames)
                {
                    var property = ErrorViewModels.FirstOrDefault(e => e.Property.Name == member);
                    if (property is null)
                    {
                        continue;
                    }

                    property.ErrorMessage = result.ErrorMessage!;
                }
            }
        }
    }
}

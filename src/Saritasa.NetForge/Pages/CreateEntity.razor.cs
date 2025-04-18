using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Saritasa.NetForge.Controls.CustomFields;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Infrastructure.Helpers;
using Saritasa.NetForge.MVVM.Navigation;
using Saritasa.NetForge.MVVM.ViewModels;
using Saritasa.NetForge.MVVM.ViewModels.CreateEntity;
using Saritasa.NetForge.MVVM.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Pages;

/// <summary>
/// Create entity page.
/// </summary>
[Route("/entities/{stringId}/create")]
public partial class CreateEntity : MvvmComponentBase<CreateEntityViewModel>
{
    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [Inject]
    private AdminOptions? AdminOptions { get; set; }

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; init; } = null!;

    private readonly List<BreadcrumbItem> breadcrumbItems = new();

    /// <inheritdoc/>
    protected override CreateEntityViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<CreateEntityViewModel>(StringId);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var adminPanelEndpoint = AdminOptions!.AdminPanelEndpoint;
        var entityDetailsEndpoint = $"{adminPanelEndpoint}/entities/{StringId}";
        var createEntityEndpoint = $"{entityDetailsEndpoint}/create";

        breadcrumbItems.Add(new BreadcrumbItem("Entities", adminPanelEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, entityDetailsEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem($"Create {ViewModel.Model.DisplayName}", createEntityEndpoint));
    }

    private void NavigateToEntityDetails()
    {
        NavigationService.NavigateTo<EntityDetailsViewModel>(parameters: StringId);
    }

    private void CreateNavigationInstance(NavigationMetadataDto navigation)
    {
        var entityType = ViewModel.Model.ClrType!;
        var entityParameter = Expression.Parameter(entityType, $"{entityType.Name}Parameter");
        var navigationExpression = Expression.Property(entityParameter, navigation.Name);

        var ctorInfo = navigation.ClrType!.GetConstructors()[0];
        var ctor = Expression.New(ctorInfo);
        var ctorAssign = Expression.Assign(navigationExpression, ctor);
        var ctorLambda = Expression.Lambda(ctorAssign, entityParameter);
        ctorLambda.Compile().DynamicInvoke(ViewModel.Model.EntityInstance);
    }

    private FieldErrorModel GetFieldError(PropertyMetadataDto property)
    {
        var fieldError = ViewModel.FieldErrorModels.FirstOrDefault(e => e.Property.Name == property.Name);
        if (fieldError is not null)
        {
            return fieldError;
        }

        fieldError = new FieldErrorModel
        {
            Property = property
        };
        ViewModel.FieldErrorModels.Add(fieldError);

        return fieldError;
    }

    private (Type ComponentType, Dictionary<string, object> Parameters) GetInputAttributes(
        PropertyMetadataDto property, FieldErrorModel fieldError)
    {
        var componentType = CustomFieldHelper.GetComponentType(property);
        var parameters = new Dictionary<string, object>
        {
            { nameof(CustomField.Property), property },
            { nameof(CustomField.EntityInstance), ViewModel.Model.EntityInstance },
            { "FieldErrorModel", fieldError }
        };

        if (property.UploadFileStrategy is not null)
        {
            parameters.Add(
                nameof(UploadImage.OnFileSelected),
                (Action<PropertyMetadataDto, IBrowserFile?>)ViewModel.HandleSelectedFile);
        }

        return (componentType, parameters);
    }
}

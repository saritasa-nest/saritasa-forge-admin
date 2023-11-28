using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Create entity page.
/// </summary>
[Route("/entities/{stringId}/create")]
public partial class CreateEntity : MvvmComponentBase<CreateEntityViewModel>
{
    private readonly List<BreadcrumbItem> breadcrumbItems = new();

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    /// <inheritdoc/>
    protected override CreateEntityViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<CreateEntityViewModel>(StringId);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var adminPanelEndpoint = AdminOptions.AdminPanelEndpoint;
        var entityDetailsEndpoint = $"{adminPanelEndpoint}/entities/{StringId}";
        var createEntityEndpoint = $"{entityDetailsEndpoint}/create";

        breadcrumbItems.Add(new BreadcrumbItem("Entities", adminPanelEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, entityDetailsEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem("Create", createEntityEndpoint));
    }

    private void NavigateToEntityDetails()
    {
        NavigationService.NavigateTo<EntityDetailsViewModel>(parameters: StringId);
    }
}

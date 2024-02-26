using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EditEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Entity details.
/// </summary>
[Route("/entities/{stringId}")]
public partial class EntityDetails : MvvmComponentBase<EntityDetailsViewModel>
{
    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; set; } = null!;

    private readonly List<BreadcrumbItem> breadcrumbItems = new();

    /// <inheritdoc/>
    protected override EntityDetailsViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<EntityDetailsViewModel>(StringId);
    }

    private Dictionary<string, object>? NonKeylessEntityDataGridAttributes { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ViewModel.CanEdit)
        {
            NonKeylessEntityDataGridAttributes = new Dictionary<string, object>
            {
                { "Hover", true },
                { "RowClass", "cursor-pointer" },
                { "RowClick", EventCallback.Factory.Create<DataGridRowClickEventArgs<object>>(this, NavigateToEditing) }
            };
        }

        var adminPanelEndpoint = AdminOptions.AdminPanelEndpoint;

        breadcrumbItems.Add(new BreadcrumbItem("Entities", href: adminPanelEndpoint));
        // Add BreadcrumbItem with the new href value because can not get StringId directly.
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, href: $"{adminPanelEndpoint}/entities/{StringId}"));
    }

    private void NavigateToCreation()
    {
        NavigationService.NavigateTo<CreateEntityViewModel>(parameters: StringId);
    }

    private void NavigateToEditing(DataGridRowClickEventArgs<object> row)
    {
        var primaryKeyValues = ViewModel.Model.Properties
            .Where(property => property.IsPrimaryKey)
            .Select(primaryKey => ViewModel.GetPropertyValue(row.Item, primaryKey).ToString()!);

        NavigationService.NavigateTo<EditEntityViewModel>(
            parameters: new[] { StringId, string.Join("--", primaryKeyValues) });
    }
}

using System.Text;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Blazor.Infrastructure;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EditEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Entity details.
/// </summary>
[Route("/entities/{stringId}")]
public partial class EntityDetails : MvvmComponentBase<EntityDetailsViewModel>, IDisposable
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

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var adminPanelEndpoint = AdminOptions.AdminPanelEndpoint;

        breadcrumbItems.Add(new BreadcrumbItem("Entities", href: adminPanelEndpoint));
        // Add BreadcrumbItem with the new href value because can not get StringId directly.
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, href: $"{adminPanelEndpoint}/entities/{StringId}"));
    }

    private void NavigateToCreation()
    {
        NavigationService.NavigateTo<CreateEntityViewModel>(parameters: StringId);
    }

    //private void NavigateToEditing()
    //{
    //    NavigationService.NavigateTo<EditEntityViewModel>(parameters: StringId);
    //}

    //protected override void OnInitialized()
    //{
    //    StateContainer.OnStateChange += StateHasChanged;
    //}

    private void NavigateToEditing(DataGridRowClickEventArgs<object> row)
    {
        var primaryKeys = new StringBuilder();

        var primaryKeyValues = new List<string>();

        foreach (var primaryKey in ViewModel.Model.Properties.Where(property => property.IsPrimaryKey))
        {
            primaryKeys.Append(ViewModel.GetPropertyValue(row.Item, primaryKey));
            primaryKeyValues.Add(ViewModel.GetPropertyValue(row.Item, primaryKey).ToString()!);
        }

        NavigationService.NavigateTo<EditEntityViewModel>(parameters: [StringId, string.Join("--", primaryKeyValues)]);
    }

    //public void Dispose()
    //{
    //    StateContainer.OnStateChange -= StateHasChanged;
    //}
}

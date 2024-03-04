using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Blazor.Controls;
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

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ILogger<EntityDetails> Logger { get; set; } = default!;

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

    private async void ShowDeleteEntityConfirmationAsync(object source)
    {
        var parameters = new DialogParameters<ConfirmationDialog>();
        parameters.Add(x => x.ContentText, "Are you sure you want to delete this record?");
        parameters.Add(x => x.ButtonText, "Yes");
        parameters.Add(x => x.Color, Color.Error);

        var result = await (await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters)).Result;
        if (!result.Canceled)
        {
            try
            {
                await ViewModel.DeleteEntityAsync(source, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to delete record due to error: {ex.Message}", Severity.Error);
                Logger.LogError("Failed to delete record due to error: {ex.Message}", ex.Message);
            }
        }
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

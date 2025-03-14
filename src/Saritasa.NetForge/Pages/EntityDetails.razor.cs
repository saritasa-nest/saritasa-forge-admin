using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Controls;
using Saritasa.NetForge.Infrastructure.Helpers;
using Saritasa.NetForge.MVVM.Navigation;
using Saritasa.NetForge.MVVM.ViewModels.CreateEntity;
using Saritasa.NetForge.MVVM.ViewModels.EditEntity;
using Saritasa.NetForge.MVVM.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Pages;

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

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

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

    private async Task ShowBulkDeleteEntitiesConfirmationAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmationDialog.ContentText), "Are you sure you want to delete these records?" },
            { nameof(ConfirmationDialog.ButtonText), "Yes" },
            { nameof(ConfirmationDialog.Color), Color.Error }
        };

        var result = await (await DialogService.ShowAsync<ConfirmationDialog>("Bulk Delete", parameters)).Result;
        if (result.Canceled)
        {
            return;
        }

        try
        {
            await ViewModel.DeleteSelectedEntitiesAsync(CancellationToken);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to delete selected records due to error: {ex.Message}", Severity.Error);
            Logger.LogError(ex, "Failed to delete selected records due to error: {Message}", ex.Message);
        }
    }

    private void NavigateToEditing(DataGridRowClickEventArgs<object> row)
    {
        var primaryKeyValues = row.Item.GetPrimaryKeyValues(ViewModel.Model.Properties);
        NavigationService.NavigateTo<EditEntityViewModel>(parameters: [StringId, primaryKeyValues]);
    }

    private string RowClassFunc(object item, int index)
    {
        if (ViewModel.SelectedEntities.Contains(item))
        {
            return "property-grid__item--selected";
        }

        return string.Empty;
    }

    private Task ExecuteCustomActionAsync()
    {
        var action = ViewModel.Model.CustomActions.FirstOrDefault(e => e.Name == ViewModel.SelectedCustomAction);
        if (action is null)
        {
            Snackbar.Add("Please select custom action you want to execute.", Severity.Error);
            Logger.LogInformation("User not selected any custom action.");

            return Task.CompletedTask;
        }

        if (ViewModel.SelectedEntities.Count == 0)
        {
            Snackbar.Add($"Please select at least one record you want to apply action {action.Name}", Severity.Error);
            Logger.LogError("User not selected any item to apply {ActionName}.", action.Name);

            return Task.CompletedTask;
        }

        try
        {
            action.Handler?.Invoke(ServiceProvider, ViewModel.SelectedEntities.AsQueryable());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to execute custom action due to error: {ex.Message}", Severity.Error);
            Logger.LogError(ex, "Failed to execute custom action. CustomAction: {CustomAction}.", action);
        }

        Snackbar.Add($"Action {action.Name} was executed.", Severity.Info);

        ViewModel.SelectedEntities.Clear();

        return Task.CompletedTask;
    }
}

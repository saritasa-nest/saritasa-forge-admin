using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Blazor.Controls;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Mvvm.Navigation;
using Saritasa.NetForge.Mvvm.ViewModels.EditEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// Edit entity page.
/// </summary>
[Route("/entities/{stringId}/{instancePrimaryKey}")]
public partial class EditEntity : MvvmComponentBase<EditEntityViewModel>
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private INavigationService NavigationService { get; set; } = null!;

    [CascadingParameter]
    private MudDialogInstance DialogInstance { get; set; }

    [Inject]
    private AdminOptions? AdminOptions { get; set; }

    /// <summary>
    /// Entity id.
    /// </summary>
    [Parameter]
    public string StringId { get; init; } = null!;

    /// <summary>
    /// Entity instance primary key.
    /// </summary>
    [Parameter]
    public string InstancePrimaryKey { get; init; } = null!;

    /// <summary>
    /// Is dialog mode flag.
    /// </summary>
    public bool IsDialogMode => DialogInstance != null;

    private readonly List<BreadcrumbItem> breadcrumbItems = new();

    /// <inheritdoc/>
    protected override EditEntityViewModel CreateViewModel()
    {
        return ViewModelFactory.Create<EditEntityViewModel>(StringId, InstancePrimaryKey);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var adminPanelEndpoint = AdminOptions!.AdminPanelEndpoint;
        var entityDetailsEndpoint = $"{adminPanelEndpoint}/entities/{StringId}";
        var editEntityEndpoint = $"{entityDetailsEndpoint}/{InstancePrimaryKey}";

        breadcrumbItems.Add(new BreadcrumbItem("Entities", adminPanelEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem(ViewModel.Model.PluralName, entityDetailsEndpoint));
        breadcrumbItems.Add(new BreadcrumbItem($"Edit {ViewModel.Model.DisplayName}", editEntityEndpoint));
    }

    private void NavigateToEntityDetails()
    {
        NavigationService.NavigateTo<EntityDetailsViewModel>(parameters: StringId);
    }

    private async void Save()
    {
        await ViewModel.UpdateEntityAsync();
        DialogInstance?.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        if (DialogInstance != null)
        {
            DialogInstance.Cancel();
            return;
        }
        NavigateToEntityDetails();
    }

    private readonly DialogOptions navigationDetailsDialogOptions = new()
    {
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Large,
        FullWidth = true,
        NoHeader = true
    };

    private Task OpenDialogAsync(DialogOptions options, DialogParameters parameters)
    {
        return DialogService.ShowAsync<NavigationEntityEditDialog>("Edit", parameters, options);
    }
}

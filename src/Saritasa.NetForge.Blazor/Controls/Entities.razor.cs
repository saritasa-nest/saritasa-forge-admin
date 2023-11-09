using MudBlazor;
using Saritasa.NetForge.Blazor.Pages;
using Saritasa.NetForge.Mvvm.ViewModels;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Entities table component.
/// </summary>
public partial class Entities : MvvmComponentBase<EntitiesViewModel>
{
    private void NavigateToDetails(TableRowClickEventArgs<EntityMetadataDto> rowEventArgs)
    {
        NavigationService.NavigateTo<EntityDetailsViewModel>(parameters: rowEventArgs.Item.Id);
    }

    private readonly TableGroupDefinition<EntityMetadataDto> groupDefinition = new()
    {
        Indentation = true,
        Expandable = true,
        Selector = e => e.Group.Name,
        IsInitiallyExpanded = false
    };
}

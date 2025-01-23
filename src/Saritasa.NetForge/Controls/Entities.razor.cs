using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Domain.UseCases.Metadata.DTOs;
using Saritasa.NetForge.MVVM.ViewModels;
using Saritasa.NetForge.MVVM.ViewModels.EntityDetails;
using Saritasa.NetForge.Pages;

namespace Saritasa.NetForge.Controls;

/// <summary>
/// Entities table component.
/// </summary>
public partial class Entities : MvvmComponentBase<EntitiesViewModel>
{
    [Inject]
    private AdminOptions? AdminOptions { get; set; }

    private void NavigateToDetails(TableRowClickEventArgs<EntityMetadataDto> rowEventArgs)
    {
        NavigationService.NavigateTo<EntityDetailsViewModel>(parameters: rowEventArgs.Item.StringId);
    }

    private TableGroupDefinition<EntityMetadataDto>? groupDefinition;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        groupDefinition = new()
        {
            Indentation = true,
            Expandable = true,
            Selector = e => e.Group.Name,
            IsInitiallyExpanded = AdminOptions!.GroupHeadersExpanded
        };
    }
}

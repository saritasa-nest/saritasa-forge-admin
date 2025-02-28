using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

public partial class NavigationEntityEditDialog : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance DialogInstance { get; set; } = null!;

    /// <summary>
    /// Entity service.
    /// </summary>
    [Inject]
    public IEntityService EntityService { get; set; } = null!;

    /// <summary>
    /// Navigation instance. Collection instance in case of navigation collection.
    /// </summary>
    [Parameter]
    public object NavigationEntityInstance { get; set; } = null!;

    /// <summary>
    /// Navigation metadata.
    /// </summary>
    [Parameter]
    public NavigationMetadataDto NavigationMetadata { get; set; } = null!;

    /// <summary>
    /// Entity instance primary key.
    /// </summary>
    public object InstancePrimaryKey { get; set; } = null!;

    /// <summary>
    /// Entity metadata.
    /// </summary>
    public GetEntityDto EntityMetadata { get; set; } = null!;

    /// <summary>
    /// Data grid reference.
    /// </summary>
    public MudDataGrid<object>? DataGrid { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var entityMetadata = await EntityService
            .GetEntityByTypeAsync(NavigationMetadata.ClrType!, CancellationToken.None);

        EntityMetadata = entityMetadata;

        var primaryKeyPropertyMetadata = entityMetadata.Properties.First(e => e.IsPrimaryKey);
        var primaryKeyProperty = NavigationMetadata.ClrType
            .GetProperty(primaryKeyPropertyMetadata.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        InstancePrimaryKey = primaryKeyProperty.GetValue(NavigationEntityInstance);
    }
}

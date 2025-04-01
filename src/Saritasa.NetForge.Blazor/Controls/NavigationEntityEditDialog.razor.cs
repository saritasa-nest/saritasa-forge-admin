using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Navigation Entity Edit Dialog form.
/// </summary>
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
    /// Entity metadata.
    /// </summary>
    [Parameter]
    public GetEntityDto EntityMetadata { get; set; } = null!;

    /// <summary>
    /// Navigation instance. Collection instance in case of navigation collection.
    /// </summary>
    [Parameter]
    public object EntityInstance { get; set; } = null!;

    /// <summary>
    /// Entity instance primary key.
    /// </summary>
    private object InstancePrimaryKey { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        var primaryKeyPropertyMetadata = EntityMetadata.Properties.First(e => e.IsPrimaryKey);
        var primaryKeyProperty = EntityMetadata.ClrType!
            .GetProperty(primaryKeyPropertyMetadata.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        InstancePrimaryKey = primaryKeyProperty!.GetValue(EntityInstance!);
    }
}

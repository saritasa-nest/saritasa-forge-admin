using Microsoft.AspNetCore.Components;
using MudBlazor;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Blazor.Controls;

public partial class NavigationDialog : ComponentBase
{
    [Inject]
    public IEntityService EntityService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public object NavigationInstance { get; set; }

    [Parameter]
    public NavigationMetadataDto NavigationMetadata { get; set; }

    [Parameter]
    public string StringId { get; set; } = null!;

    public IEnumerable<PropertyMetadataDto> Properties { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        var entityType = NavigationMetadata.ClrType!.GetGenericArguments().FirstOrDefault()
                         ?? NavigationMetadata.ClrType;

        var entity = await EntityService.GetEntityByTypeAsync(entityType, CancellationToken.None);
        Properties = entity.Properties;
    }

    void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    void Cancel()
    {
        MudDialog.Cancel();
    }
}

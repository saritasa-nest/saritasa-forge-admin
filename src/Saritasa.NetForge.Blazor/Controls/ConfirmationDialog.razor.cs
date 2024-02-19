using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Delete entity confirmation dialog.
/// </summary>
public partial class ConfirmationDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
}

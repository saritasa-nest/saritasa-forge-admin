using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Saritasa.NetForge.Blazor.Controls;

/// <summary>
/// Confirmation dialog.
/// </summary>
public partial class ConfirmationDialog
{
    /// <summary>
    /// Mud Dialog instance.
    /// </summary>
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// Content of the dialog.
    /// </summary>
    [Parameter]
    public string ContentText { get; set; } = null!;

    /// <summary>
    /// Content of the button inside the dialog.
    /// </summary>
    [Parameter]
    public string ButtonText { get; set; } = null!;

    /// <summary>
    /// Color of the button inside the dialog.
    /// </summary>
    [Parameter]
    public Color Color { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

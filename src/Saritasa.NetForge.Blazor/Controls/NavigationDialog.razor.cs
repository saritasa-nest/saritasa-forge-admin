using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Saritasa.NetForge.Blazor.Controls;

public partial class NavigationDialog : ComponentBase
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    void Cancel()
    {
        MudDialog.Cancel();
    }
}

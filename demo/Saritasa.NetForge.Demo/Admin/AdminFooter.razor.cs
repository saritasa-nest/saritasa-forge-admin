using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Saritasa.NetForge.Demo.Admin;

/// <summary>
/// Footer for admin panel.
/// </summary>
public partial class AdminFooter : ComponentBase
{
    [Inject]
    private IJSRuntime Js { get; set; } = null!;

    /// <summary>
    /// Visitors count.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public int VisitorsCount { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        await Js.InvokeVoidAsync("setVisitsCount");
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Saritasa.NetForge.Demo.Views.Admin;

/// <summary>
/// Footer for admin panel.
/// </summary>
public partial class AdminFooter : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? module;

    [Inject]
    private ShopDbContext DbContext { get; set; } = null!;

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
        if (firstRender)
        {
            return;
        }

        module = await Js.InvokeAsync<IJSObjectReference>("import", "../Views/Admin/AdminFooter.razor.js");
        await module.InvokeVoidAsync("incrementVisitsCount");
        await DbContext.Database.MigrateAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (module is not null)
        {
            try
            {
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Required in Blazor Server but can be removed in WebAssembly.
            }
        }
    }
}
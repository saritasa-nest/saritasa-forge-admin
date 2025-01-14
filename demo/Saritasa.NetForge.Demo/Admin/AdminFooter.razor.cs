using Microsoft.AspNetCore.Components;

namespace Saritasa.NetForge.Demo.Admin;

/// <summary>
/// Footer for admin panel.
/// </summary>
public partial class AdminFooter : ComponentBase
{
    /// <summary>
    /// Visitors count.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public int VisitorsCount { get; set; }
}
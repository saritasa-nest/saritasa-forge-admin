using Microsoft.AspNetCore.Components;

namespace Saritasa.NetForge.Blazor.Pages;

public partial class CustomBodyContent
{
    [Parameter]
    public RenderFragment? Content { get; set; }
}

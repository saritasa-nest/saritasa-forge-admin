using Saritasa.NetForge.Blazor.Infrastructure.Helpers;

namespace Saritasa.NetForge.Blazor.Shared;

/// <summary>
/// Main layout page.
/// </summary>
public class MainLayoutComponent : Microsoft.AspNetCore.Components.LayoutComponentBase
{
    /// <summary>
    /// Get home page route.
    /// </summary>
    /// <returns>Home page route template.</returns>
    protected static string GetHomePageRoute()
        => RouteHelper.GetRoute<Pages.Index>();
}

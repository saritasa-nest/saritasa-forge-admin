using Saritasa.NetForge.Infrastructure.Helpers;

namespace Saritasa.NetForge.Shared;

/// <summary>
/// Main layout page.
/// </summary>
public class MainLayoutComponent : AdminBaseLayout
{
    /// <summary>
    /// Get home page route.
    /// </summary>
    /// <returns>Home page route template.</returns>
    protected static string GetHomePageRoute()
        => RouteHelper.GetRoute<Pages.Index>();
}

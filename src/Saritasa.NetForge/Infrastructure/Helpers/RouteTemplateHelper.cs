using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing.Template;

namespace Saritasa.NetForge.Infrastructure.Helpers;

/// <summary>
/// Helper for route templates.
/// </summary>
public static class RouteTemplateHelper
{
    /// <summary>
    /// Get route template for the component.
    /// </summary>
    /// <typeparam name="TComponent">Component.</typeparam>
    /// <returns>Route template.</returns>
    public static RouteTemplate GetRouteTemplate<TComponent>()
        where TComponent : ComponentBase
    {
        var componentType = typeof(TComponent);
        return GetRouteTemplate(componentType);
    }

    /// <summary>
    /// Get route template for component type.
    /// </summary>
    /// <param name="componentType">Component type.</param>
    /// <returns>Route template.</returns>
    /// <exception cref="InvalidOperationException">Route attribute for component was not found.</exception>
    public static RouteTemplate GetRouteTemplate(Type componentType)
    {
        var routeAttribute = componentType.GetCustomAttribute<RouteAttribute>()
                             ?? throw new InvalidOperationException("Route attribute for component was not found.");
        return TemplateParser.Parse(routeAttribute.Template);
    }
}

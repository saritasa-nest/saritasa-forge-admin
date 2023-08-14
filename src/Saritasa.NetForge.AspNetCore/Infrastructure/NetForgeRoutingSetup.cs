using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Saritasa.NetForge.AspNetCore.Controllers;

namespace Saritasa.NetForge.AspNetCore.Infrastructure;

/// <summary>
/// Setups routing for the admin panel.
/// </summary>
internal class NetForgeRoutingSetup
{
    private readonly IEndpointRouteBuilder routeBuilder;

    /// <summary>
    /// Constructor.
    /// </summary>
    internal NetForgeRoutingSetup(IEndpointRouteBuilder routeBuilder)
    {
        this.routeBuilder = routeBuilder;
    }

    internal void SetupApiRoutes(string apiEndpoint)
    {
        routeBuilder.MapControllerRoute(name: "NetForgeEntitiesMetadata", pattern: $"{apiEndpoint}/metadata/entities",
            new { controller = "Metadata", action = nameof(MetadataController.GetEntities) });
    }

    internal void SetupRazorViewRoutes(string endpoint)
    {
        routeBuilder.MapControllerRoute(name: "NetForge", pattern: endpoint,
            new { controller = "NetForge", action = nameof(NetForgeController.Index) });
    }
}

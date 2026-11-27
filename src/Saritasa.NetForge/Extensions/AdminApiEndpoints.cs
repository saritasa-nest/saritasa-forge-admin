using Saritasa.NetForge.Domain.UseCases.Interfaces;

namespace Saritasa.NetForge.Extensions;

/// <summary>
/// Read-only JSON API consumed by the Angular admin UI.
/// </summary>
internal static class AdminApiEndpoints
{
    /// <summary>
    /// Map admin API endpoints.
    /// </summary>
    /// <param name="endpoints">Endpoint route builder.</param>
    public static void MapNetForgeApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/entities", GetEntities);
    }

    private static async Task<IResult> GetEntities(IEntityService entityService, CancellationToken cancellationToken)
    {
        var entities = await entityService.SearchEntitiesAsync(cancellationToken);
        return Results.Ok(entities.Select(entity => new
        {
            entity.StringId,
            entity.PluralName,
            entity.Description,
            Group = new { entity.Group.Name, entity.Group.Description }
        }));
    }
}

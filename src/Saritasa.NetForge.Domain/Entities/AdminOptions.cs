namespace Saritasa.NetForge.Domain.Entities;

/// <summary>
/// Admin panel options.
/// </summary>
public class AdminOptions
{
    /// <summary>
    /// Admin panel endpoint.
    /// </summary>
    public string AdminPanelEndpoint { get; set; } = "/netforge";

    /// <summary>
    /// NetForge API endpoint.
    /// </summary>
    public string ApiEndpoint { get; set; } = "/netforge-api";
}

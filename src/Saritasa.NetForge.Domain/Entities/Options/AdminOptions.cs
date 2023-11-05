namespace Saritasa.NetForge.Domain.Entities.Options;

/// <summary>
/// Admin panel options.
/// </summary>
public class AdminOptions
{
    /// <summary>
    /// Admin panel endpoint.
    /// </summary>
    public string AdminPanelEndpoint { get; set; } = "/admin";

    /// <summary>
    /// Header title for the admin panel web page.
    /// </summary>
    public string AdminPanelHeaderTitle { get; set; } = "NetForge Admin";

    /// <summary>
    /// HTML title for the admin panel web page.
    /// </summary>
    public string AdminPanelHtmlTitle { get; set; } = "NetForge Admin - Index";

    /// <summary>
    /// Options for configuring entities in the admin panel.
    /// </summary>
    public ICollection<EntityOptions> EntityOptionsList { get; } = new List<EntityOptions>();
}

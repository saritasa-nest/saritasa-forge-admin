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
    /// Url that can be configured in admin panel.
    /// </summary>
    public string SiteUrl { get; set; } = "/";

    /// <summary>
    /// Whether the properties of entities are displayed in Title Case.
    /// By default, property names like "ContactInfo" will be displayed as "Contact Info".
    /// </summary>
    public bool TitleCaseProperties { get; set; } = true;

    /// <summary>
    /// Header title for the admin panel web page.
    /// </summary>
    public string AdminPanelHeaderTitle { get; set; } = "NetForge Admin";

    /// <summary>
    /// HTML title for the admin panel web page.
    /// </summary>
    public string AdminPanelHtmlTitle { get; set; } = "NetForge Admin - Main";

    /// <summary>
    /// Options for configuring entities in the admin panel.
    /// </summary>
    public ICollection<EntityOptions> EntityOptionsList { get; } = new List<EntityOptions>();

    /// <summary>
    /// Groups that entities belong to.
    /// </summary>
    public ICollection<EntityGroup> EntityGroupsList { get; } = new List<EntityGroup>();
}

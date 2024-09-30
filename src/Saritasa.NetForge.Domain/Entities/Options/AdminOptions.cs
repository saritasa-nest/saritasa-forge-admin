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
    /// Roles with access to the admin panel.
    /// </summary>
    public ICollection<string> AdminPanelAccessRoles { get; set; } = new List<string>();

    /// <summary>
    /// Custom authentication function for admin panel access.
    /// </summary>
    public Func<IServiceProvider, Task<bool>>? CustomAuthFunction { get; set; }

    /// <summary>
    /// Options for configuring entities in the admin panel.
    /// </summary>
    public ICollection<EntityOptions> EntityOptionsList { get; } = new List<EntityOptions>();

    /// <summary>
    /// Groups that entities belong to.
    /// </summary>
    public ICollection<EntityGroup> EntityGroupsList { get; } = new List<EntityGroup>();

    /// <summary>
    /// Whether entity group header should be expanded.
    /// </summary>
    public bool GroupHeadersExpanded { get; set; } = true;

    /// <summary>
    /// Path to folder that contains static files.
    /// </summary>
    public string StaticFilesFolder { get; set; } = "wwwroot";

    /// <summary>
    /// Path to folder where to store media files.
    /// </summary>
    public string MediaFolder { get; set; } = "media";

    /// <summary>
    /// Maximum image size in megabytes.
    /// </summary>
    public int MaxImageSizeInMb { get; set; } = 10;

    /// <summary>
    /// Maximum number of characters. Exceeded characters will be truncated.
    /// </summary>
    public int TruncationMaxCharacters { get; set; } = 50;

    /// <summary>
    /// The date format for displaying dates.
    /// </summary>
    public string? DateFormat { get; set; }
}

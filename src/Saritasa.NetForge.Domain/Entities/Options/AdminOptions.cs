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
    /// Options for configuring entities in the admin panel.
    /// </summary>
    public ICollection<EntityOptions> EntityOptionsList { get; } = new List<EntityOptions>();

    /// <summary>
    /// Groups that entities belong to.
    /// </summary>
    public ICollection<EntityGroup> EntityGroupsList { get; } = new List<EntityGroup>();
}

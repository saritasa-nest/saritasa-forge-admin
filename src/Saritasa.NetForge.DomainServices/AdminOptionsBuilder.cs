using Saritasa.NetForge.Domain.Entities;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builds the admin options.
/// </summary>
public class AdminOptionsBuilder
{
    private AdminOptions Options { get; set; } = new();

    /// <summary>
    /// ORM-specific options builder.
    /// </summary>
    public IOrmOptionsBuilder? OrmOptionsBuilder { get; set; }

    /// <summary>
    /// Set the URL on which the admin panel will be hosted.
    /// </summary>
    /// <param name="url">URL.</param>
    public AdminOptionsBuilder UseEndpoint(string url)
    {
        Options.AdminPanelEndpoint = url;
        return this;
    }

    /// <summary>
    /// Get options for the admin panel.
    /// </summary>
    public AdminOptions Create()
    {
        return Options;
    }
}

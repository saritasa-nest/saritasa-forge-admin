using Saritasa.NetForge.Domain.Entities;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Builds the admin options.
/// </summary>
public interface IAdminOptionsBuilder
{
    /// <summary>
    /// ORM-specific options builder.
    /// </summary>
    IOrmOptionsBuilder? OrmOptionsBuilder { get; set; }

    /// <summary>
    /// Set the URL on which the admin panel will be hosted.
    /// </summary>
    /// <param name="url">URL.</param>
    IAdminOptionsBuilder UseEndpoint(string url);

    /// <summary>
    /// Get options for the admin panel.
    /// </summary>
    AdminOptions Create();
}

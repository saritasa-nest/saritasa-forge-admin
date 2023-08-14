using Saritasa.NetForge.Domain.Entities;

namespace Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Builds the admin options.
/// </summary>
public interface IAdminOptionsBuilder
{
    /// <summary>
    /// Options for the admin panel.
    /// </summary>
    AdminOptions Options { get; set; }

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
    /// Set the URL on which the NetForge API will be hosted.
    /// </summary>
    /// <param name="url">URL.</param>
    IAdminOptionsBuilder UseApiEndpoint(string url);
}

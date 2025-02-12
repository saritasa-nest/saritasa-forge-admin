using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Entities;

/// <summary>
/// Client.
/// </summary>
public class Client
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Entities;

/// <summary>
/// Project.
/// </summary>
public class Project
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

    /// <summary>
    /// The client who is owner of the project.
    /// </summary>
    public Client Client { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Models;

/// <summary>
/// Department.
/// </summary>
public class Department
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
    /// Employees who work in the department.
    /// </summary>
    public List<Employee> Employees { get; set; } = [];

    /// <summary>
    /// Projects developed by the department.
    /// </summary>
    public List<Project> Projects { get; set; } = [];
}

using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.EfCoreMetadataServiceTests.NavigationMetadata.Models;

/// <summary>
/// Employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    [MaxLength(150)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// The department where the employee works.
    /// </summary>
    public Department Department { get; set; } = null!;

    /// <summary>
    /// Supervisor.
    /// </summary>
    public Employee Supervisor { get; set; } = null!;

    /// <summary>
    /// Employees who this employee supervises.
    /// </summary>
    public List<Employee> Supervised { get; set; } = [];
}
